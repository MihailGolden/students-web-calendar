function loadData() {
    $.ajax({
        url: "/Calendar/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Title + '</td>';
                html += '<td>' + item.Description + '</td>';
                html += '<td><a href="/Calendar/Open?calendarID=' + item.ID + '">Open</a> ';
                html += '<a href="#" onclick="return getbyID(' + item.ID + ')">Update</a> ';
                html += '<a href="#" onclick="Delete(' + item.ID + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('.list').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Add() {
    if (!Validate()) {
        return false;
    }
    var list = {
        Title: $('#Title').val(),
        Description: $('#Description').val()
    };
    $.ajax({
        url: "/Calendar/Create",
        data: JSON.stringify(list),
        type: "POST",
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (result) {
            $('#myModal').modal('hide');
            loadData();
        }, onSuccess: function (result) {
            $.validator.unobtrusive.parse($(result));
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Update() {
    if (!Validate()) {
        return false;
    }
    var Obj = {
        ID: $('#CalendarID').val(),
        Title: $('#Title').val(),
        Description: $('#Description').val()
    };
    $.ajax({
        url: "/Calendar/Update",
        data: JSON.stringify(Obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#CalendarID').val("");
            $('#Title').val("");
            $('#Description').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearText() {
    $('#CalendarID').val("");
    $('#Title').val("");
    $('#Description').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
}

function getbyID(ID) {
    $.ajax({
        url: "/Calendar/getbyID/" + ID,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#CalendarID').val(result.ID);
            $('#Title').val(result.Title);
            $('#Description').val(result.Description);
            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function Delete(ID) {
    var check = confirm("Are you sure want to delete this Calendar?");
    if (check) {
        $.ajax({
            url: "/Calendar/Delete/" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

function Validate() {
    var isValid = true;
    if ($("#Title").val() == "") {
        $("#Title").css("border-color", "Red");
        isValid = false;
    } else {
        $("#Title").css("border-color", "lightgrey");
    }
    if ($("#Description").val() == "") {
        $("#Description").css("border-color", "Red");
        isValid = false;
    } else {
        $("#Description").css("border-color", "lightgrey");
    }
    return isValid;
}

$(document).ready(function () {
    loadData();
});