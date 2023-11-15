using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Plan7.Helper.Hubs;
using System.Security.Claims;
using Shared.Dto.Detail;
using Shared.Dto.Detail.DetailChild;
using Shared.Enums;
using Shared.Dto.Detail.Filters;
using ServiceLayer.Details.Services.Interfaces;

namespace Plan7.Hubs.DetailHubs;

/// <summary>
/// Хаб деталей
/// </summary>
[Authorize]
public class DetailHub : Hub
{
    private readonly IDetailChildService _detailChildRepository;
    private readonly IDetailService _detailRepository;
    private readonly IDetailCountService _count;
    private readonly static ConnectionMapping _connections = new();
    private readonly static EditMapping _editMapping = new();
    private readonly static EditMapping _childEditMapping = new();

    public DetailHub(IDetailService detailRepository, 
        IDetailChildService detailChildRepository, 
        IDetailCountService count)
    {
        _count = count;
        _detailRepository = detailRepository;
        _detailChildRepository = detailChildRepository;
    }

    /// <summary>
    /// Получаем список изменяемых записей
    /// </summary>
    /// <returns>Список изменямых записей в виде List</returns>
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
    /// Проверяем, редактируется ли состав
    /// </summary>
    /// <param name="blockId">Id детали, чей состав блокируется</param>
    /// <returns>true если редактируется, false, если нет</returns>
    [Authorize]
    public async Task CheckEditChild(int blockId)
    {
        await Clients.Others.SendAsync("BlockEditChild", _childEditMapping.CheckEdit(blockId));
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
    /// Получаем сотрудника, который редактирует состав детали
    /// </summary>
    /// <param name="id">Id детали, информацию о сотруднике который редактирует ее состав мы хотим узнать</param>
    /// <returns>Информацию о сотруднике - "ФИО - табельный_номер"</returns>
    [Authorize]
    public async Task WhoEditDetailChild(int id)
    {
        await Clients.Client(Context.ConnectionId).SendAsync("WhoEditDetail", _childEditMapping.GetEditor(id));
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
    /// Блокировка состава на редактирование
    /// </summary>
    /// <param name="blockId">Id детали, чей состав нужно заблокировать</param>
    /// <returns>Отправялем другим пользователям, что состав нужно заблокировать</returns>
    [Authorize]
    public async Task BlockEditChild(int blockId)
    {
        _childEditMapping.Add(_connections.GetConnections(Context.ConnectionId), blockId);
        await Clients.Others.SendAsync("BlockEditChild", blockId);
    }

    /// <summary>
    /// Открываем запись на редактирование
    /// </summary>
    /// <param name="openId">Id записи, которую нужно открыть на редактирование</param>
    /// <returns>Отправляем другим пользователям, подключенным к хабу, id записи, которую нужно открыть на редактирование</returns>
    [Authorize]
    public async Task OpenEdit(int openId)
    {
        _editMapping.Remove(_connections.GetConnections(Context.ConnectionId));
        await Clients.Others.SendAsync("OpenEdit", openId);
    }

    /// <summary>
    /// Открываем состав на редактирование
    /// </summary>
    /// <param name="openId">Id детали, чей состав нужно открыть на редактирование</param>
    /// <returns>Отправляем другим пользователям, подключенным к хабу, id детали, чей состав нужно открыть на редактирование</returns>
    [Authorize]
    public async Task OpenEditChild(int openId)
    {
        _childEditMapping.Remove(_connections.GetConnections(Context.ConnectionId));
        await Clients.Others.SendAsync("OpenEditChild", openId);
    }

    /// <summary>
    /// Добаление записи
    /// </summary>
    /// <param name="addedId">Id добавленной записи</param>
    /// <returns>Отправляем другим пользователям данные о записи, которая была добавлена</returns>
    [Authorize]
    public async Task SendAdded(int addedId)
    {
        GetDetailInfoWithIdDto? detail = await _detailRepository.GetFirstAsync(addedId);
        if (_detailRepository.HasErrors || detail == null)
            return;
        await Clients.Others.SendAsync("Added", detail.Id, detail.SerialNumber, detail.Title, detail.DetailTypeId, await _count.GetAllAsync());
    }

    /// <summary>
    /// Добавление состава в деталь
    /// </summary>
    /// <param name="fatherId">Id детали, в состав которой была добавлена деталь</param>
    /// <param name="childId">Id детали, которую добавили в состав</param>
    /// <returns>Отпавляем другим пользователям информацию о добавленной детали в состав fatherId</returns>
    [Authorize]
    public async Task SendAddedChild(int fatherId, int childId)
    {
        GetDetailChildDto? child = await _detailChildRepository.GetFirstChildAsync(fatherId, childId);
        if (_detailChildRepository.HasErrors || child == null)
            return;
        await Clients.Others.SendAsync("AddedChild", fatherId, child.Id, child.SerialNumber, child.Title, child.Number, child.Count, child.Unit);
    }

    /// <summary>
    /// Добавление детали в заменяемость
    /// </summary>
    /// <param name="detailId">Id детали, в которой была добавлена заменяемость</param>
    /// <param name="detailReplaceabilities">Список деталей, которые были добавлены в заменяемости детали detailId</param>
    /// <returns>Детали(ь) которые были добавлены в заменямость</returns>
    [Authorize]
    public async Task SendAddedReplaceabilities(int detailId, List<BaseIdSerialTitleDto> detailReplaceabilities)
    {
        await Clients.Others.SendAsync("AddedReplaceabilities", detailId, detailReplaceabilities);
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="deletedId">Id удаляемой записи</param>
    /// <returns>Отправляем другим пользователям Id записи, которую нужно удалить</returns>
    [Authorize]
    public async Task SendDeleted(int deletedId, string text = "", int detailTypeId = 0, SerialNumberOrTitleFilter searchOptions = default)
    {
        _editMapping.Remove(_connections.GetConnections(Context.ConnectionId));
        await Clients.Others.SendAsync("Deleted", deletedId, await _count.GetAllAsync(new GetAllDetailFilters() { Text = text, DetailTypeId = detailTypeId, SearchOptions = searchOptions }));
    }

    /// <summary>
    /// Удаление детали из состава детали
    /// </summary>
    /// <param name="fatherId">Деталь, в составе которой произошло удаление</param>
    /// <param name="childId">Деталь, которую удалили из состава</param>
    /// <returns>Отправка другим подключенным к хабу пользователям fatherId детали и childId</returns>
    [Authorize]
    public async Task SendDeletedChild(int fatherId, int childId)
    {
        _childEditMapping.Remove(_connections.GetConnections(Context.ConnectionId));
        await Clients.Others.SendAsync("DeletedChild", fatherId, childId);
    }

    /// <summary>
    /// Удаление детали из заменяемости
    /// </summary>
    /// <param name="childId">Деталь, которую удалили из заменямости</param>
    /// <returns>Отправка другим пользователям childId для удаления из состава</returns>
    [Authorize]
    public async Task SendDeletedReplaceabilities(int childId)
    {
        await Clients.Others.SendAsync("DeletedReplaceabilities", childId);
    }

    /// <summary>
    /// Обновление записи
    /// </summary>
    /// <param name="changedId">Id записи, которая была обновлена</param>
    /// <returns>Отправляем другим пользователям актуальную информацию</returns>
    [Authorize]
    public async Task SendChanged(int changedId)
    {
        GetDetailInfoWithIdDto? detail = await _detailRepository.GetFirstAsync(changedId);
        if (_detailRepository.HasErrors || detail == null)
            return;
        int id = _editMapping.GetEdit(_connections.GetConnections(Context.ConnectionId));
        _editMapping.Remove(_connections.GetConnections(Context.ConnectionId));
        await OpenEdit(id);
        await Clients.Others.SendAsync("Changed", detail.Id, detail.SerialNumber, detail.Title, detail.DetailTypeId, detail.UnitId, detail.Weight);
        await Clients.Others.SendAsync("ChangedReplaceabilities", detail.Id, detail.SerialNumber, detail.Title);
        await Clients.Others.SendAsync("ChangedChild", detail.Id, detail.SerialNumber, detail.Title, detail.UnitId);
    }

    /// <summary>
    /// Редактирование детали в составе
    /// </summary>
    /// <param name="fatherId">Деталь, в составе которой произошло редактирование</param>
    /// <param name="childId">Деталь, которую отредактировали в составе</param>
    /// <returns>Измненная деталь в составе детали fatherId</returns>
    [Authorize]
    public async Task SendChangedChild(int fatherId, int childId)
    {
        GetDetailChildDto? child = await _detailChildRepository.GetFirstChildAsync(fatherId, childId);
        if (_detailChildRepository.HasErrors || child == null)
            return;
        await Clients.Others.SendAsync("ChangedChild", fatherId, child.Id, child.SerialNumber, child.Title, child.Number, child.Count, child.Unit);
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
        int detailEditId = _editMapping.GetEdit(user);
        int detailChildEditId = _childEditMapping.GetEdit(user);
        if (detailEditId != 0)
        {
            _editMapping.Remove(user);
            OpenEdit(detailEditId);
        }
        if (detailChildEditId != 0)
        {
            _childEditMapping.Remove(user);
            OpenEditChild(detailEditId);
        }
        return base.OnDisconnectedAsync(exception);
    }
}
