using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Plan7.Helper.Hubs;
using System.Security.Claims;
using Shared.Dto.Equipment;
using ServiceLayer.Equipments.Services.Interfaces;
using AutoMapper;

namespace Plan7.Hubs;

/// <summary>
/// Хаб станков
/// </summary>
[Authorize]
public class EquipmentHub : Hub
{
    private readonly IEquipmentService _equipmentRepository;
    private readonly IEquipmentCountService _count;
    private readonly static ConnectionMapping _connections = new();
    private readonly static EditMapping _editMapping = new();

    public EquipmentHub(IEquipmentService equipmentRepository, IEquipmentCountService count)
    {
        _equipmentRepository = equipmentRepository;
        _count = count;
    }

    /// <summary>
    /// Получаем список изменяемых записей
    /// </summary>
    /// <returns>Список изменямых записей в виде List<int></returns>
    [Authorize]
    public async Task Loading()
    {
        string user = _connections.GetConnections(Context.ConnectionId);
        List<int> edits = null!;
        if (_editMapping.Count > 0)
        {
            var myEdit = _editMapping.GetEdit(user);
            if (myEdit != 0)
                _editMapping.Remove(user);
            edits = _editMapping.GetEdits();
        }
        await Clients.Client(Context.ConnectionId).SendAsync("Loading", edits);
    }

    /// <summary>
    /// Блокируем запись на редактирование
    /// </summary>
    /// <param name="blockId">Id блокируемой записи</param>
    /// <returns>Отправляем другим подключенным к данному хабу пользователям id записи, которая была заблокирова на редактирование</returns>
    [Authorize]
    public async Task BlockEdit(int blockId)
    {
        _editMapping.Add(_connections.GetConnections(Context.ConnectionId), blockId);
        await Clients.Others.SendAsync("BlockEdit", blockId);
    }

    /// <summary>
    /// Получаем сотрудника, который редактирует запись
    /// </summary>
    /// <param name="id">Id записи, информацию о сотруднике который ее редактирует мы хотим узнать</param>
    /// <returns>Информацию о сотруднике - "ФИО - табельный_номер"</returns>
    [Authorize]
    public async Task WhoEditDetail(int id)
    {
        await Clients.Client(Context.ConnectionId).SendAsync("WhoEditDetail", _editMapping.GetEditor(id));
    }

    /// <summary>
    /// Открываем запись на редактирование
    /// </summary>
    /// <param name="openId">Id записи, которую нужно открыть на редактирование</param>
    /// <returns>Отправляем другим пользователям id записи, которую нужно открыть на редактирование</returns>
    [Authorize]
    public async Task OpenEdit(int openId)
    {
        _editMapping.Remove(_connections.GetConnections(Context.ConnectionId));
        await Clients.Others.SendAsync("OpenEdit", openId);
    }

    /// <summary>
    /// Добавление записи
    /// </summary>
    /// <param name="addedId">Id добавленной записи</param>
    /// <returns>Отправляем другим пользователям данные о записи, которая была добавлена</returns>
    [Authorize]
    public async Task SendAdded(int addedId)
    {
        var equipment = await _equipmentRepository.GetFirstAsync(addedId);
        if (_equipmentRepository.HasErrors || equipment == null)
            return;
        await Clients.Others.SendAsync("Added", equipment.Id, equipment.Title, equipment.SerialNumber, await _count.GetAllAsync());
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="deletedId">Id удаляемой записи</param>
    /// <returns>Отправляем другим пользователям Id записи, которую нужно удалить</returns>
    [Authorize]
    public async Task SendDeleted(int deletedId)
    {
        _editMapping.Remove(_connections.GetConnections(Context.ConnectionId));
        await Clients.Others.SendAsync("Deleted", deletedId, await _count.GetAllAsync());
    }

    /// <summary>
    /// Обновление записи
    /// </summary>
    /// <param name="changedId">Id записи, которая была обновлена</param>
    /// <returns>Отправляем другим пользователям актуальную информацию</returns>
    [Authorize]
    public async Task SendChanged(int changedId)
    {
        var equipment = await _equipmentRepository.GetFirstAsync(changedId);
        if (_equipmentRepository.HasErrors || equipment == null)
            return;
        int id = _editMapping.GetEdit(_connections.GetConnections(Context.ConnectionId));
        _editMapping.Remove(_connections.GetConnections(Context.ConnectionId));
        await OpenEdit(id);
        await Clients.Others.SendAsync("Changed", equipment.Id, equipment.Title, equipment.SerialNumber);
    }

    /// <summary>
    /// Подключение к хабу
    /// </summary>
    /// <returns></returns>
    [Authorize]
    public override Task OnConnectedAsync()
    {
        _connections.Add(Context.ConnectionId, $"{Context.User!.FindFirstValue(ClaimTypes.Name)!} - {Context.User!.FindFirstValue("Login")!}");
        return base.OnConnectedAsync();
    }

    /// <summary>
    /// Отключение пользователя
    /// </summary>
    /// <param name="exception">Ошибка которая привела к отключению, если она есть</param>
    /// <returns></returns>
    [Authorize]
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        string user = _connections.GetConnections(Context.ConnectionId);
        _connections.Remove(user);
        int id = _editMapping.GetEdit(user);
        if (id != 0)
        {
            _editMapping.Remove(user);
            OpenEdit(id);
        }
        return base.OnDisconnectedAsync(exception);
    }
}
