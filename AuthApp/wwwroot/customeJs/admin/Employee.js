var Employee = {
    intiTbl: function (isSearch = false, pageSize = 10, reload = false) {

        var Id = $("#employee-search-id").val().trim();
        var Name = $("#employee-search-name").val().trim();
        var Email = $("#employee-search-email").val().trim();
        var PhoneNumber = $("#employee-search-phoneNumber").val().trim();
        var RoleId = ($("#employee-search-roleId").val() != "") ? ($("#employee-search-roleId").val()) : (0);

        var data = {
            Id: Id,
            Name: Name,
            Email: Email,
            PhoneNumber: PhoneNumber,
            RoleId: RoleId,
            PageSize: pageSize
        };
        var columns = [
            { data: "id" },
            { data: "firstName" },
            { data: "email" },
            { data: "phoneNumber" },
            {
                "render": function (data, type, row) {
                    var roles = row.roles.filter(role => role.name !== "Doctor").map(role => role.name).join(',');
                    return roles;

                }

            }
        ];


        columns.push(
            {
                "render": function (data, type, row) {
                    /*  if (model.isEdit) {*/
                    var str = '<a href="javascript:void(0);" data-bs-toggle="modal" data-bs-target="#modals-slide-in" title="edit" onclick="Employee.openEditEmployeeModal(`' + row.id + '`)" class="item-edit">' +
                        feather.icons['edit'].toSvg({ class: 'font-medium-4 me-50' }) +
                        '</a>'

                    /*   }*/

                    /*  if (model.isDelete) {*/

                    str += '<a href="javascript:void(0);" title="delete"  class="item-edit">' +
                        feather.icons['x'].toSvg({ class: 'font-medium-4' }) +
                        '</a>' +
                        '</div>';

                    /*}*/
                    return str;

                }


            });

        var url = "/Admin/Employee/List";
        var tableId = "#employeeTbl";
        var pageSize = pageSize;
        var buttons =
            /*model.isAdd ?*/
            [
                {

                    text: feather.icons['plus'].toSvg({ class: 'me-50 font-small-4' }) + 'Add New Employee',
                    className: 'btn-primary',
                    attr: {
                        'data-bs-toggle': 'modal',
                        'data-bs-target': '#modals-slide-in'
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-secondary');
                        $(node).addClass('btn-primary');
                        $(node).click(function () {

                            Employee.openEditEmployeeModal(0);

                        })
                    }
                }

            ] /*: [];*/

        var columnsDefs = [];
        general.intiDataTable(tableId, url, data, columns, columnsDefs, buttons, pageSize);
    },
    openEditEmployeeModal: function (Id) {
        var data = {
            Id: Id
        };
        general.callAjax("/Admin/Employee/AddEdit", data, function (response) {
            $("#addEditDoctorModal").html(response);
            if (Id == 0) {
                $('#doctorModalTitle').text('Add New Employee');
            } else {
                $('#doctorModalTitle').text('Edit Employee');
            }
        }, false);

    },
    SubmitAddEditEmployeeForm: function (event) {
        event.preventDefault();
        var formData = new FormData();
        var $form = $('#addEditEmployeeForm');
        var ControlArray = $form.serializeArray();
        for (var i = 0; i < ControlArray.length; i++) {
            formData.append(ControlArray[i].name, ControlArray[i].value);
        }

        general.callAjaxOnForm("/Admin/Employee/AddEdit", formData, function (response) {

            if (response.status == 1) {
                console.log(response);
                toastr['info'](response.serverInfo.message + '!! 😀', 'success', {
                    positionClass: 'toast-top-right',
                    rtl: true,
                    timeOut: 1000,
                    onHidden: function () {
                        $("#modals-slide-in").modal('hide');
                        Employee.intiTbl();
                    }
                });
            } else {
                console.log(response);
            }
        }, true);

    }
}