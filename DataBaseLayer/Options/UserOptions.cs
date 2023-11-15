using DB.Model.UserInfo;
using Shared.Enums;

namespace DatabaseLayer.Options;

public static class UserOptions
{
    public static IQueryable<User> UserSearch(this IQueryable<User> users, UserSearchOptions searchOptions, string text)
    {
        if (string.IsNullOrEmpty(text)) 
            return users;

        return searchOptions switch
        {
            UserSearchOptions.ForFfl => users.Where(u => u.FFL.Contains(text)),
            UserSearchOptions.ForProfNumber => users.Where(u => u.ProfessionNumber.Contains(text)),
            _ => users,
        };
    }

    public static IQueryable<User> UserOrder(this IQueryable<User> users, UserOrderOptions orderOptions = default, KindOfOrder kindOfOrder = default)
    {
        return orderOptions switch
        {
            UserOrderOptions.ForProfNumber => kindOfOrder switch
            {
                KindOfOrder.Up => users.OrderBy(u => u.ProfessionNumber),
                KindOfOrder.Down => users.OrderByDescending(u => u.ProfessionNumber),
                _ => users.OrderBy(u => u.Id),
            },
            UserOrderOptions.ForFfl => kindOfOrder switch
            {
                KindOfOrder.Up => users.OrderBy(u => u.FFL),
                KindOfOrder.Down => users.OrderByDescending(u => u.FFL),
                _ => users.OrderBy(u => u.Id),
            },
            UserOrderOptions.ForSubdiv => kindOfOrder switch
            {
                KindOfOrder.Up => users.OrderBy(u => u.Subdivision.Title),
                KindOfOrder.Down => users.OrderByDescending(u => u.Subdivision.Title),
                _ => users.OrderBy(u => u.Id),
            },
            UserOrderOptions.ForProf => kindOfOrder switch
            {
                KindOfOrder.Up => users.OrderBy(u => u.Profession.Title),
                KindOfOrder.Down => users.OrderByDescending(u => u.Profession.Title),
                _ => users.OrderBy(u => u.Id),
            },
            _ => users.OrderBy(u => u.Id)
        };
    }

    public static IQueryable<User> UserProfessionOrder(this IQueryable<User> users, UserOrderFromProfOptions orderOptions = default, KindOfOrder kindOfOrder = default)
    {
        return orderOptions switch
        {
            UserOrderFromProfOptions.ForFfl => kindOfOrder switch
            {
                KindOfOrder.Up => users.OrderBy(u => u.FFL),
                KindOfOrder.Down => users.OrderByDescending(u => u.FFL),
                _ => users.OrderBy(u => u.Id),
            },
            UserOrderFromProfOptions.ForProfNumber => kindOfOrder switch
            {
                KindOfOrder.Up => users.OrderBy(u => u.ProfessionNumber),
                KindOfOrder.Down => users.OrderByDescending(u => u.ProfessionNumber),
                _ => users.OrderBy(u => u.Id),
            },
            UserOrderFromProfOptions.ForSubdiv => kindOfOrder switch
            {
                KindOfOrder.Up => users.OrderBy(u => u.Subdivision.Title),
                KindOfOrder.Down => users.OrderByDescending(u => u.Subdivision.Title),
                _ => users.OrderBy(u => u.Id),
            },
            _ => users.OrderBy(u => u.Id)
        };
    }

    public static IQueryable<User> UserSubdivOrder(this IQueryable<User> users, UserOrderFromSubdivOptions orderOptions = default, KindOfOrder kindOfOrder = default)
    {
        return orderOptions switch
        {
            UserOrderFromSubdivOptions.ForFfl => kindOfOrder switch
            {
                KindOfOrder.Up => users.OrderBy(u => u.FFL),
                KindOfOrder.Down => users.OrderByDescending(u => u.FFL),
                _ => users.OrderBy(u => u.Id),
            },
            UserOrderFromSubdivOptions.ForProfNumber => kindOfOrder switch
            {
                KindOfOrder.Up => users.OrderBy(u => u.ProfessionNumber),
                KindOfOrder.Down => users.OrderByDescending(u => u.ProfessionNumber),
                _ => users.OrderBy(u => u.Id),
            },
            UserOrderFromSubdivOptions.ForProf => kindOfOrder switch
            {
                KindOfOrder.Up => users.OrderBy(u => u.Profession.Title),
                KindOfOrder.Down => users.OrderByDescending(u => u.Profession.Title),
                _ => users.OrderBy(u => u.Id),
            },
            _ => users.OrderBy(u => u.Id)
        };
    }

    public static IQueryable<User> UserSelectFromStatus(this IQueryable<User> users, int statusId)
    {
        if (statusId <= 0)
            return users;
        return users.Where(u => u.StatusId == statusId);
    }
}