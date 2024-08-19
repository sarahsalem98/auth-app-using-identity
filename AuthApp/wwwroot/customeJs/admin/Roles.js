var Roles={

    SubmitAddRolesPermissions: function (event) {
        event.preventDefault();
        var form = document.getElementById("addRoleForm");
        var formData = new FormData(form);
        var permissions = [];
        var modalRoleNameInput = document.getElementById("modalRoleName");
        var modalRoleIdInput = document.getElementById("modalRoleId");
        var Type = document.getElementById("roleType");
        var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();
        form.querySelectorAll('input[type="checkbox"]:checked:not(#selectAll)').forEach(function (checkbox) {
            var pageNoInput = checkbox.closest('tr').querySelector('input[name^="pageNo"]');
            var operationNoInput = checkbox.closest('div').querySelector('input[name^="operationNo"]');
            if (pageNoInput && operationNoInput) {
                var pageNo = pageNoInput.value;
                var operationNo = operationNoInput.value;
                permissions.push({
                    pageNo: pageNo,
                    operationNo:operationNo
                });
            }
        })
        var data = {
            type: Type.value,
            roleId: modalRoleIdInput.value,
            roleName: modalRoleNameInput.value,
            __RequestVerificationToken: antiForgeryToken,
            permissions: permissions
        };
        general.callAjax("/Admin/Roles/AddRolePermissions", data,
            function (response) {
                console.log(response);
                if (response.status == 1) {
                    toastr['info']( response.serverInfo.message + '!! 😀', 'success', {
                        positionClass: 'toast-top-right',
                        rtl: true,
                        timeOut: 1000,
                        onHidden: function () {
                            window.location.href = '/Admin/Roles/Index?Type=' + data.type;
                        }
                    });
                } else {
                    console.log(response);
                }
            }, true
        );

    },
    OpenEditRoleModal: function (roleId, roleName,roleType) {
        var data = {
            Type: roleType,
            roleId: roleId,
        };
        general.callAjax("/Admin/Roles/AddEditRolePermissions", data, function (response) {
            $('#addEditRolePermissions').html(response);
            if (roleId == 0) {
                $('.role-title').text('Add New Role');
            } else {
                $('.role-title').text('Edit Role' + '-(' + roleName +' )');
            }
             Roles.Init();
   
        },false);


    },
    Init: function () {
        var form = document.getElementById("addRoleForm");
        if (!form) return; 
        form.addEventListener("submit", Roles.SubmitAddRolesPermissions);

        var selectAllCheckbox = document.getElementById("selectAll");
        var individualCheckboxes = form.querySelectorAll('input[type="checkbox"]:not(#selectAll)');

        selectAllCheckbox.addEventListener('change', function () {
            individualCheckboxes.forEach(function (checkbox) {
                checkbox.checked = selectAllCheckbox.checked;
            });
        });

        individualCheckboxes.forEach(function (checkbox) {
            checkbox.addEventListener('change', function () {
                if (!checkbox.checked) {
                    selectAllCheckbox.checked = false;
                } else {
                    var allChecked = Array.from(individualCheckboxes).every(function (cb) {
                        return cb.checked;
                    });
                    selectAllCheckbox.checked = allChecked;
                }
            });
        });
    }

}
document.addEventListener("DOMContentLoaded", function () {
    Roles.Init();
});