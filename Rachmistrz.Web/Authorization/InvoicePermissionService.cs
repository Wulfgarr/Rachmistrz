using Rachmistrz.Web.Constants;
using Rachmistrz.Web.DTOs;
using Rachmistrz.Web.Enums;

namespace Rachmistrz.Web.Authorization;

public class InvoicePermissionService
{
    public bool CanViewInvoice(
        InvoiceDetailsDto invoice,
        string userId,
        int? userBranchId,
        IEnumerable<string> roles)
    {
        if (HasAnyRole(roles, RoleNames.Admin, RoleNames.Accounting))
        {
            return true;
        }

        if (HasRole(roles, RoleNames.BranchManager))
        {
            return invoice.BranchId == userBranchId;
        }

        if (HasRole(roles, RoleNames.Employee))
        {
            return invoice.CreatedByUserId == userId;
        }

        return false;
    }

    public bool CanEditInvoice(IEnumerable<string> roles)
    {
        return HasAnyRole(roles, RoleNames.Admin, RoleNames.Accounting);
    }

    public bool CanAddInvoice(IEnumerable<string> roles)
    {
        return HasAnyRole(
            roles,
            RoleNames.Admin,
            RoleNames.Accounting,
            RoleNames.Employee);
    }

    public bool CanChangeStatus(
        InvoiceDetailsDto invoice,
        InvoiceStatus newStatus,
        string userId,
        int? userBranchId,
        IEnumerable<string> roles)
    {
        if (HasRole(roles, RoleNames.Admin))
        {
            return true;
        }

        if (HasRole(roles, RoleNames.Accounting))
        {
            return newStatus is InvoiceStatus.Submitted
                or InvoiceStatus.UnderReview
                or InvoiceStatus.Booked
                or InvoiceStatus.Paid
                or InvoiceStatus.Cancelled;
        }

        if (HasRole(roles, RoleNames.BranchManager))
        {
            return invoice.BranchId == userBranchId
                && newStatus is InvoiceStatus.Approved or InvoiceStatus.Rejected;
        }

        if (HasRole(roles, RoleNames.Employee))
        {
            return invoice.CreatedByUserId == userId
                && newStatus is InvoiceStatus.Submitted or InvoiceStatus.Cancelled;
        }

        return false;
    }

    public bool CanCommentInvoice(
        InvoiceDetailsDto invoice,
        string userId,
        int? userBranchId,
        IEnumerable<string> roles)
    {
        return CanViewInvoice(invoice, userId, userBranchId, roles);
    }

    private static bool HasRole(IEnumerable<string> roles, string role)
    {
        return roles.Contains(role);
    }

    private static bool HasAnyRole(IEnumerable<string> roles, params string[] allowedRoles)
    {
        return roles.Any(allowedRoles.Contains);
    }
}