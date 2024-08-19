var general = {
    intiDataTable: function (tableId, ajaxUrl, ajaxdata = {}, columns, columnsDefs, buttons, pageSize) {
        $(tableId).DataTable({
            searching: false,
            destroy: true,
            serverSide: true,
            processing: true,
            ajax: {
                "url": ajaxUrl,
                "type": "POST",
                "data": function (d) {
                    return $.extend({}, d, ajaxdata);
                },
                "dataSrc": function (result) {
                    console.log(result);
                    return result.data
                },
                error: function (xhr, error, thrown) {
                    console.error("AJAX Error: ", error); // Log AJAX errors
                    console.error("Response: ", xhr.responseText);
                }
            },
            columns: columns,
            columnDefs: columnsDefs,
            pageLength: pageSize,
            dom:
                '<"d-flex justify-content-between align-items-center header-actions mx-2 row mt-75"' +
                '<"col-sm-12 col-lg-4 d-flex justify-content-center justify-content-lg-start" l>' +
                '<"col-sm-12 col-lg-8 ps-xl-75 ps-0"<"dt-action-buttons d-flex align-items-center justify-content-center justify-content-lg-end flex-lg-nowrap flex-wrap"<"me-1"f>B>>' +
                '>t' +
                '<"d-flex justify-content-between mx-2 row mb-1"' +
                '<"col-sm-12 col-md-6"i>' +
                '<"col-sm-12 col-md-6"p>' +
                '>',

            buttons: buttons



        });



    }
    , callAjax: function (url, data, onSuccess, isPost = false) {
        console.log("ajax" + data);
        $.ajax({
            url: url,
            type: (isPost) ? ("POST") : ("GET"),
            data: data,
            success: onSuccess,
            error: function (error) {
                console.log(error);
            }
        });
    },
    callAjaxOnForm: function (url, data, onSuccess, isPost = false) {
        console.log("ajax" + data);
        $.ajax({
            url: url,
            type: (isPost) ? ("POST") : ("GET"),
            contentType: false,
            processData: false,
            data: data,
            success: onSuccess,
            error: function (error) {
                console.log(error);
            }
        });
    }

}