﻿var Doctor = {
    intiTbl: function (isSearch = false, pageSize = 10, reload = false) {

        var Id = $("#doctor-search-id").val().trim();
        var Name = $("#doctor-search-name").val().trim();
        var Email = $("#doctor-search-email").val().trim();
        var PhoneNumber = $("#doctor-search-phoneNumber").val().trim();
        var RoleId = ($("#doctor-search-roleId").val() != "") ? ($("#doctor-search-roleId").val()) : (0);

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
            { data: "user.firstName" },
            { data: "user.email" },
            { data: "user.phoneNumber" },
            {
                "render": function (data, type, row) {
                    var roles = row.user.roles.filter(role => role.name!=="Doctor").map(role => role.name).join(',');
                    return roles;
                 
                }

            }
        ];


        columns.push(
            {
                "render": function (data, type, row) {
                  /*  if (model.isEdit) {*/
                    var str = '<a href="javascript:void(0);" data-bs-toggle="modal" data-bs-target="#modals-slide-in" title="edit" onclick="Doctor.openEditDoctorModal(`' + row.id + '`)" class="item-edit">' +
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

        var url = "/Admin/Doctor/List";
        var tableId = "#doctorTbl";
        var pageSize = pageSize;
        var buttons =
            /*model.isAdd ?*/
                [
                    {

                        text: feather.icons['plus'].toSvg({ class: 'me-50 font-small-4' }) + 'Add New Doctor',
                        className:'btn-primary',
                        attr: {
                            'data-bs-toggle': 'modal',
                            'data-bs-target': '#modals-slide-in'
                        },
                        init: function (api, node, config) {
                            $(node).removeClass('btn-secondary');
                            $(node).addClass('btn-primary');
                            $(node).click(function () {

                                Doctor.openEditDoctorModal(0);

                            })
                        }
                    }

                ] /*: [];*/

        var columnsDefs = [];
        general.intiDataTable(tableId, url, data, columns, columnsDefs, buttons, pageSize);

    },
    openEditDoctorModal: function (doctorId) {
        var data = {
            doctorId: doctorId
        };
        general.callAjax("/Admin/Doctor/AddEdit", data, function (response) {
            $("#addEditDoctorModal").html(response);
            if (doctorId == 0) {
                $('#doctorModalTitle').text('Add New Doctor');
            } else {
                $('#doctorModalTitle').text('Edit Doctor');
            }
        }, false);

    },

    delete: function (Id) {
        console.log("delete");
        var data = {};
        data.Id = Id
        general.callAjax("/admin/employee/Delete", data,
            function (response) {
                if (response.status == 1) {
                    toastr['info']('the class deleted successfully !! 😀', 'Success', {
                        positionClass: 'toast-top-right',
                        rtl: true,
                        timeOut: 1000,
                        onHidden: function () {
                            window.location.href = '/admin/employee/index';
                        }
                    });

                } else {
                    alert("somthing went wrong");
                }
            }, true);
    },
    SubmitAddEditDoctorForm: function (event) {
        event.preventDefault(); 
        var formData = new FormData();
        var $form = $('#addEditDoctorForm');
        var ControlArray = $form.serializeArray();
        for (var i = 0; i < ControlArray.length; i++) {
            formData.append(ControlArray[i].name, ControlArray[i].value);
        }

        general.callAjaxOnForm("/Admin/Doctor/AddEdit", formData, function (response) {

            if (response.status == 1) {
                console.log(response);
                toastr['info'](response.serverInfo.message + '!! 😀', 'success', {
                    positionClass: 'toast-top-right',
                    rtl: true,
                    timeOut: 1000,
                    onHidden: function () {
                        $("#modals-slide-in").modal('hide');
                        Doctor.intiTbl();
                    }
                });
            } else {
                console.log(response);
            }
        }, true);

    }
}