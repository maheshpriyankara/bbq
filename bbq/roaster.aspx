<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="roaster.aspx.cs" Inherits="bbq.roaster" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <title>HRIS</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="icon" href="favicon.ico" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" id="theme" href="css/theme-default.css" />

    <link href="jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="css/styleloading.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.13.0/jquery-ui.min.js"></script>
    <script type="text/javascript" src="js/bootstrap-min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script type="text/javascript">

        function myMethod4() {

            if (document.getElementById('text_epfNo_').value != '' || document.getElementById('text_attendanceId').value != '') {


                $.ajax({
                    url: "https://localhost:44341/api/ProcessSalary/getProcessQueueTimesheetUser?empname=" + document.getElementById('text_epfNo_').value + "&empid=" + document.getElementById('text_attendanceId').value,
                    success: function (data) {
                        const label = document.getElementById("label_status");
                        const label2 = document.getElementById("label_status2");

                        label.innerHTML = "Process Completed";
                        label.style.color = "blue";
                        label2.innerHTML = "Process Completed";
                        label2.style.color = "orange";

                        $.each(data, function (index, value) {

                            if (value.Line == 'Single') {
                                label.innerHTML = "Process Required...";
                                label.style.color = "red";

                            } else if (value.Line == 'Full') {
                                label2.innerHTML = "Full Month Processing....";
                                label2.style.color = "red";

                            }
                        });

                    },
                    complete: function () {
                        $("#loading").css("visibility", "hidden");
                    }
                });

            }
        }
        window.onload = function () {
            myMethod4();
            setInterval(myMethod4, 1000);
        };
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            SearchText();
        });
        function SearchText() {

            $("#text_employee").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "roaster.aspx/getEmployee",
                        data: "{'empName':'" + document.getElementById('text_employee').value + "','line':'" + document.getElementById('list_lines').value + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            alert(result);
                        }
                    });
                },
                focus: function (event, ui) {

                    return false; // prevent the default behavior of the focus event
                },

                select: function (event, ui) {
                    var table = $("#table tbody");
                    table.empty();
                    var table2 = $("#table4 tbody");
                    table2.empty();
                    $.ajax({
                        type: "POST",
                        url: "roaster.aspx/employeeSelected",
                        data: "{'empName':'" + ui.item.value + "','year':'" + document.getElementById('date_search').value + "','month':'" + document.getElementById('list_month').value + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d == '') {
                               
                                alert('Sorry, Salary not Processed for Selected Employee')
                                document.getElementById('text_employee').value = ui.item.value.split('(')[0]

                            } else {
                                var result = data.d.split('_')
                                document.getElementById('text_employee').value = ""
                                document.getElementById('text_epfNo_').value = result[0]
                                document.getElementById('text_attendanceId').value = result[1]
                                document.getElementById('text_employeeName').value = result[2]
                                document.getElementById('text_department').value = result[3]

                                var result = ui.item.value.split('(')[1]
                                var epfno = result.split(',')[0].split('-')[1]
                                if (document.getElementById('text_department').value == 'CLEANING DEPARTMENT') {
                                    loadAttendanceCleaning(epfno)

                                } else {
                                    loadRoasterMonth(epfno)
                                }

                            }
                            //   loadLeaveBalance(epfno)
                        },
                        error: function (result) {

                        }
                    });

                }
            }

            );
        }
    </script>
    <script type="text/javascript">
        function loadRoasterMonth(epfno) {
            loadAttRow(epfno)
            $.ajax({
                url: "https://localhost:44341/api/roaster/getroasterAttendancemonth?empname=" + epfno + "&year=" + document.getElementById('date_search').value + "&month=" + document.getElementById('list_month').value + "&empid=" + document.getElementById('text_attendanceId').value,
                beforeSend: function () {
                    $("#loading").css("visibility", "visible");
                },
                success: function (data) {
                    var table = $("#table tbody");
                    table.empty();
                    document.getElementById('text_workHours').value = ""
                    document.getElementById('text_paycut').value = ""
                    document.getElementById('text_ot').value = ""
                    document.getElementById('text_extraot').value = ""
                    document.getElementById('text_workDays').value = ""
                    document.getElementById('text_nopayDays').value = ""
                    $.ajax({
                        url: "https://localhost:44341/api/ProcessSalary/CheckEmployee?epfno=" + epfno + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&empid=" + document.getElementById('text_attendanceId').value,

                        success: function (data2) {
                            if (data2 == 1) {
                                $.each(data, function (index, value) {
                                    var row = $("<tr>");
                                    row.append($("<td>").text(value.InDate));
                                    row.append($("<td>").text(value.RoasterFirst));
                                    row.append($("<td>").text(value.ShiftOneInTime));
                                    row.append($("<td>").text(value.ShiftOneOutTime));
                                    row.append($("<td>").text(value.RoasterSecond));
                                    row.append($("<td>").text(value.ShiftTwoInTime));
                                    row.append($("<td>").text(value.ShiftTwoOutTime));
                                    row.append($("<td>").text(value.WorkHours));
                                    row.append($("<td>").text(value.PayCut));
                                    row.append($("<td>").text(value.OverTime15));
                                    row.append($("<td>").text(value.ExtraOT));

                                    var checkbox = $("<input>").attr(
                                        {
                                            type: "checkbox",
                                            checked: value.DO,
                                            disabled: true
                                        }
                                    )
                                    var checkboxCell = $("<td>").append(checkbox);
                                    row.append(checkboxCell);
                                    ///
                                    checkbox = $("<input>").attr(
                                        {
                                            type: "checkbox",
                                            checked: value.Leave,
                                            disabled: true
                                        }
                                    )
                                    checkboxCell = $("<td>").append(checkbox);
                                    row.append(checkboxCell);
                                    ///
                                    checkbox = $("<input>").attr(
                                        {
                                            type: "checkbox",
                                            checked: value.LeaveHalf,
                                            disabled: true
                                        }
                                    )
                                    checkboxCell = $("<td>").append(checkbox);
                                    row.append(checkboxCell);
                                    ///
                                    checkbox = $("<input>").attr(
                                        {
                                            type: "checkbox",
                                            checked: value.SleepD,
                                            disabled: true
                                        }
                                    )
                                    checkboxCell = $("<td>").append(checkbox);
                                    row.append(checkboxCell);
                                    ///
                                    checkbox = $("<input>").attr(
                                        {
                                            type: "checkbox",
                                            checked: value.LeLe,
                                            disabled: true
                                        }
                                    )
                                    checkboxCell = $("<td>").append(checkbox);
                                    row.append(checkboxCell);
                                    ///
                                    checkbox = $("<input>").attr(
                                        {
                                            type: "checkbox",
                                            checked: value.MSf,
                                            disabled: true
                                        }
                                    )
                                    checkboxCell = $("<td>").append(checkbox);
                                    row.append(checkboxCell);
                                    ///
                                    checkbox = $("<input>").attr(
                                        {
                                            type: "checkbox",
                                            checked: value.NoPay,
                                            disabled: true
                                        }
                                    )
                                    checkboxCell = $("<td>").append(checkbox);
                                    row.append(checkboxCell);
                                    ///
                                    table.append(row);
                                });
                            } else {
                                if (data2 == 2) {
                                } else {
                                }
                                $.ajax({
                                    url: "https://localhost:44341/api/roaster/getroasterlist",
                                    success: function (roasterListData) {
                                        var roasterFirstOptions = [];
                                        $.each(roasterListData, function (index, value) {
                                            roasterFirstOptions.push(value.Roaster);
                                        });
                                        $.each(data, function (index, value) {

                                            var row = $("<tr>");
                                            row.append($("<td>").text(value.InDate));

                                            var roasterFirstSelect = $("<select>").addClass("roaster-select");
                                            $.each(roasterFirstOptions, function (index, optionValue) {
                                                roasterFirstSelect.append($("<option>").attr("value", optionValue).text(optionValue));
                                            });
                                            roasterFirstSelect.val(value.RoasterFirst);
                                            var roasterFirstCell = $("<td>");
                                            roasterFirstCell.append(roasterFirstSelect);
                                            row.append(roasterFirstCell);
                                            roasterFirstSelect.change(function () {

                                                var selectedRoaster = $(this).val(); // get the selected value
                                                var selectedDate = $(this).closest('tr').find('td:first').text();

                                                $.ajax({
                                                    url: "https://localhost:44341/api/roaster/updateRoaster?empname=" + epfno + "&date=" + selectedDate + "&roaster=" + selectedRoaster + "&mode=1" + "&empid=" + document.getElementById('text_attendanceId').value,
                                                    beforeSend: function () {
                                                        $("#loading").css("visibility", "visible");
                                                    },
                                                    type: "PUT",
                                                    success: function (data) {
                                                        $.ajax({
                                                            url: "https://localhost:44341/api/ProcessSalary/PushEmployeeDay?epfno=" + epfno + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&requestFrom=Shift Update (First)&user_=_&ip=" + selectedDate + "&empid=" + document.getElementById('text_attendanceId').value + "&empid=" + document.getElementById('text_attendanceId').value,
                                                            type: "PUT",
                                                            success: function (data) {
                                                            }
                                                        });

                                                    },
                                                    error: function (xhr, textStatus, errorThrown) {
                                                        if (xhr.status == 400) {
                                                            alert("Duplicate entry found for " + dateTime);
                                                        } else {
                                                            alert("Error adding record. Please try again." + xhr.status);
                                                        }
                                                    }, complete: function () {
                                                        $("#loading").css("visibility", "hidden");
                                                    }
                                                });
                                            });

                                            row.append($("<td>").text(value.ShiftOneInTime));
                                            row.append($("<td>").text(value.ShiftOneOutTime));

                                            var roasterSecondSelect = $("<select>").addClass("roaster-select");
                                            $.each(roasterFirstOptions, function (index, optionValue) {
                                                roasterSecondSelect.append($("<option>").attr("value", optionValue).text(optionValue));
                                            });
                                            roasterSecondSelect.val(value.RoasterSecond);
                                            var roasterSecondCell = $("<td>");
                                            roasterSecondCell.append(roasterSecondSelect);
                                            row.append(roasterSecondCell);
                                            roasterSecondSelect.change(function () {
                                                var selectedRoaster = $(this).val(); // get the selected value
                                                var selectedDate = $(this).closest('tr').find('td:first').text();;
                                                $.ajax({
                                                    beforeSend: function () {
                                                        $("#loading").css("visibility", "visible");
                                                    },
                                                    url: "https://localhost:44341/api/roaster/updateRoaster?empname=" + epfno + "&date=" + selectedDate + "&roaster=" + selectedRoaster + "&mode=2" + "&empid=" + document.getElementById('text_attendanceId').value,
                                                    type: "PUT",
                                                    success: function (data) {
                                                        $.ajax({
                                                            url: "https://localhost:44341/api/ProcessSalary/PushEmployeeDay?epfno=" + epfno + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&requestFrom=Shift Update (Second)&user_=_&ip=" + selectedDate + "&empid=" + document.getElementById('text_attendanceId').value,
                                                            type: "PUT",
                                                            success: function (data) {
                                                            }
                                                        });
                                                    },
                                                    error: function (xhr, textStatus, errorThrown) {
                                                        if (xhr.status == 400) {
                                                            alert("Duplicate entry found for " + dateTime);
                                                        } else {
                                                            alert("Error adding record. Please try again." + xhr.status);
                                                        }
                                                    }, complete: function () {
                                                        $("#loading").css("visibility", "hidden");
                                                    }
                                                });
                                            });

                                            row.append($("<td>").text(value.ShiftTwoInTime));
                                            row.append($("<td>").text(value.ShiftTwoOutTime));
                                            row.append($("<td>").text(value.WorkHours));
                                            row.append($("<td>").text(value.PayCut));
                                            row.append($("<td>").text(value.OverTime15));
                                            var extraOTCell = $("<td>");
                                            var extraOTInput = $("<input>").attr(
                                                {
                                                    type: "text",
                                                    class: "form-control timepicker24",
                                                    value: value.ExtraOT
                                                }
                                            );
                                            extraOTCell.append(extraOTInput);
                                            row.append(extraOTCell);

                                            extraOTInput.on("change", function () {

                                                var value = $(this).val();
                                                var regex24 = /^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$/i;

                                                var selectedDate = $(this).closest('tr').find('td:first').text();
                                                if (regex24.test(value)) {

                                                    $.ajax({
                                                        beforeSend: function () {
                                                            $("#loading").css("visibility", "visible");
                                                        },
                                                        url: "https://localhost:44341/api/attendance/AddExtraOT?empname=" + epfno + "&date=" + selectedDate + "&ot=" + value + "&empid=" + document.getElementById('text_attendanceId').value,
                                                        type: "POST",
                                                        success: function (data) {
                                                            $.ajax({
                                                                url: "https://localhost:44341/api/ProcessSalary/PushEmployeeDay?epfno=" + epfno + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&requestFrom=Manual Attendance&user_=_&ip=" + selectedDate + "&empid=" + document.getElementById('text_attendanceId').value,
                                                                type: "PUT",
                                                                success: function (data) {
                                                                    $("#loading").css("visibility", "hidden");
                                                                }
                                                            });

                                                        },
                                                        error: function (xhr, textStatus, errorThrown) {

                                                            alert("Error adding record. Please try again." + xhr.status);
                                                            $("#loading").css("visibility", "hidden");
                                                        }
                                                    });
                                                } else {
                                                    alert("Invalid time format.");
                                                    $("#loading").css("visibility", "hidden");
                                                }
                                            });
                                            var checkbox = $("<input>").attr(
                                                {
                                                    type: "checkbox",
                                                    checked: value.DO
                                                }
                                            )
                                            var checkboxCell = $("<td>").append(checkbox);
                                            row.append(checkboxCell);
                                            checkbox.change(function () {
                                                var row = $(this).closest('tr');
                                                var checkboxState = $(this).is(':checked');

                                                var row = $(this).closest('tr');
                                                var checkboxColumnIndex = row.find('td').index($(this).parent());
                                                var columnName = row.closest('table').find('tr:first th').eq(checkboxColumnIndex).text();

                                                chnageCheckBox(epfno, row.find('td:eq(0)').text(), columnName, checkboxState);
                                            });
                                            ///
                                            checkbox = $("<input>").attr(
                                                {
                                                    type: "checkbox",
                                                    checked: value.Leave
                                                }
                                            )
                                            checkboxCell = $("<td>").append(checkbox);
                                            row.append(checkboxCell);
                                            checkbox.change(function () {
                                                var row = $(this).closest('tr');
                                                var checkboxState = $(this).is(':checked');

                                                var row = $(this).closest('tr');
                                                var checkboxColumnIndex = row.find('td').index($(this).parent());
                                                var columnName = row.closest('table').find('tr:first th').eq(checkboxColumnIndex).text();

                                                chnageCheckBox(epfno, row.find('td:eq(0)').text(), columnName, checkboxState);

                                            });
                                            ///
                                            checkbox = $("<input>").attr(
                                                {
                                                    type: "checkbox",
                                                    checked: value.LeaveHalf
                                                }
                                            )
                                            checkboxCell = $("<td>").append(checkbox);
                                            row.append(checkboxCell);
                                            checkbox.change(function () {
                                                var row = $(this).closest('tr');
                                                var checkboxState = $(this).is(':checked');

                                                var row = $(this).closest('tr');
                                                var checkboxColumnIndex = row.find('td').index($(this).parent());
                                                var columnName = row.closest('table').find('tr:first th').eq(checkboxColumnIndex).text();

                                                chnageCheckBox(epfno, row.find('td:eq(0)').text(), columnName, checkboxState);

                                            });
                                            ///
                                            checkbox = $("<input>").attr(
                                                {
                                                    type: "checkbox",
                                                    checked: value.SleepD
                                                }
                                            )
                                            checkboxCell = $("<td>").append(checkbox);
                                            row.append(checkboxCell);
                                            checkbox.change(function () {
                                                var row = $(this).closest('tr');
                                                var checkboxState = $(this).is(':checked');

                                                var row = $(this).closest('tr');
                                                var checkboxColumnIndex = row.find('td').index($(this).parent());
                                                var columnName = row.closest('table').find('tr:first th').eq(checkboxColumnIndex).text();

                                                chnageCheckBox(epfno, row.find('td:eq(0)').text(), columnName, checkboxState);
                                            });
                                            ///
                                            checkbox = $("<input>").attr(
                                                {
                                                    type: "checkbox",
                                                    checked: value.LeLe
                                                }
                                            )
                                            checkboxCell = $("<td>").append(checkbox);
                                            row.append(checkboxCell);
                                            checkbox.change(function () {
                                                var row = $(this).closest('tr');
                                                var checkboxState = $(this).is(':checked');

                                                var row = $(this).closest('tr');
                                                var checkboxColumnIndex = row.find('td').index($(this).parent());
                                                var columnName = row.closest('table').find('tr:first th').eq(checkboxColumnIndex).text();

                                                chnageCheckBox(epfno, row.find('td:eq(0)').text(), columnName, checkboxState);
                                            });
                                            ///
                                            checkbox = $("<input>").attr(
                                                {
                                                    type: "checkbox",
                                                    checked: value.MSf
                                                }
                                            )
                                            checkboxCell = $("<td>").append(checkbox);
                                            row.append(checkboxCell);
                                            checkbox.change(function () {
                                                var row = $(this).closest('tr');
                                                var checkboxState = $(this).is(':checked');

                                                var row = $(this).closest('tr');
                                                var checkboxColumnIndex = row.find('td').index($(this).parent());
                                                var columnName = row.closest('table').find('tr:first th').eq(checkboxColumnIndex).text();

                                                chnageCheckBox(epfno, row.find('td:eq(0)').text(), columnName, checkboxState);
                                            });
                                            ///
                                            checkbox = $("<input>").attr(
                                                {
                                                    type: "checkbox",
                                                    checked: value.PreOT
                                                }
                                            )
                                            checkboxCell = $("<td>").append(checkbox);
                                            row.append(checkboxCell);
                                            checkbox.change(function () {
                                                var row = $(this).closest('tr');
                                                var checkboxState = $(this).is(':checked');

                                                var row = $(this).closest('tr');
                                                var checkboxColumnIndex = row.find('td').index($(this).parent());
                                                var columnName = row.closest('table').find('tr:first th').eq(checkboxColumnIndex).text();

                                                chnageCheckBox(epfno, row.find('td:eq(0)').text(), columnName, checkboxState);
                                            });
                                            ///
                                            checkbox = $("<input>").attr(
                                                {
                                                    type: "checkbox",
                                                    checked: value.NoPay,
                                                    disabled: true
                                                }
                                            )
                                            checkboxCell = $("<td>").append(checkbox);
                                            row.append(checkboxCell);

                                            table.append(row);


                                        });
                                    }
                                });

                            }
                            loadLeaveBalance(epfno);
                            $("#loading").css("visibility", "hidden");
                        },
                        complete: function () {

                        }
                    });


                },
                complete: function () {
                }
            });

        }
    </script>
    <script type="text/javascript">
        function loadAttendanceCleaning(epfno) {
            $.ajax({
                url: "https://localhost:44341/api/attendance/getattmonthCleaning?empname=" + epfno + "&year=" + document.getElementById('date_search').value + "&month=" + document.getElementById('list_month').value + "&empid=" + document.getElementById('text_attendanceId').value,
                beforeSend: function () {
                    $("#loading").css("visibility", "visible");
                },
                success: function (data) {
                    var table4 = $("#table4 tbody");
                    table4.empty();
                    $.ajax({
                        url: "https://localhost:44341/api/ProcessSalary/CheckEmployee?epfno=" + epfno + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&empid=" + document.getElementById('text_attendanceId').value,

                        success: function (data2) {
                            if (data2 == 1) {
                                $.each(data, function (index, value) {

                                    var row = $("<tr>");
                                    row.append($("<td>").text(value.Date));
                                    checkbox = $("<input>").attr(
                                        {
                                            type: "checkbox",
                                            checked: value.FirstShift,
                                            disabled: true
                                        }
                                    )
                                    checkboxCell = $("<td>").append(checkbox);
                                    row.append(checkboxCell);
                                    checkbox.change(function () {
                                        var row = $(this).closest('tr');
                                        var checkboxState = $(this).is(':checked');

                                        var row = $(this).closest('tr');
                                        var checkboxColumnIndex = row.find('td').index($(this).parent());
                                        var columnName = row.closest('table').find('tr:first th').eq(checkboxColumnIndex).text();

                                        chnageCheckBox2(epfno, row.find('td:eq(0)').text(), columnName, checkboxState);

                                    });
                                    ///
                                    checkbox = $("<input>").attr(
                                        {
                                            type: "checkbox",
                                            checked: value.SecondShift,
                                            disabled: true
                                        }
                                    )
                                    checkboxCell = $("<td>").append(checkbox);
                                    row.append(checkboxCell);
                                    checkbox.change(function () {
                                        var row = $(this).closest('tr');
                                        var checkboxState = $(this).is(':checked');

                                        var row = $(this).closest('tr');
                                        var checkboxColumnIndex = row.find('td').index($(this).parent());
                                        var columnName = row.closest('table').find('tr:first th').eq(checkboxColumnIndex).text();

                                        chnageCheckBox2(epfno, row.find('td:eq(0)').text(), columnName, checkboxState);

                                    });
                                    table4.append(row);


                                });
                            } else {
                                $.each(data, function (index, value) {

                                    var row = $("<tr>");
                                    row.append($("<td>").text(value.Date));
                                    checkbox = $("<input>").attr(
                                        {
                                            type: "checkbox",
                                            checked: value.FirstShift
                                        }
                                    )
                                    checkboxCell = $("<td>").append(checkbox);
                                    row.append(checkboxCell);
                                    checkbox.change(function () {
                                        var row = $(this).closest('tr');
                                        var checkboxState = $(this).is(':checked');

                                        var row = $(this).closest('tr');
                                        var checkboxColumnIndex = row.find('td').index($(this).parent());
                                        var columnName = row.closest('table').find('tr:first th').eq(checkboxColumnIndex).text();

                                        chnageCheckBox2(epfno, row.find('td:eq(0)').text(), columnName, checkboxState);

                                    });
                                    ///
                                    checkbox = $("<input>").attr(
                                        {
                                            type: "checkbox",
                                            checked: value.SecondShift
                                        }
                                    )
                                    checkboxCell = $("<td>").append(checkbox);
                                    row.append(checkboxCell);
                                    checkbox.change(function () {
                                        var row = $(this).closest('tr');
                                        var checkboxState = $(this).is(':checked');

                                        var row = $(this).closest('tr');
                                        var checkboxColumnIndex = row.find('td').index($(this).parent());
                                        var columnName = row.closest('table').find('tr:first th').eq(checkboxColumnIndex).text();

                                        chnageCheckBox2(epfno, row.find('td:eq(0)').text(), columnName, checkboxState);

                                    });
                                    table4.append(row);


                                });
                            }
                            loadLeaveBalance(epfno);
                            $("#loading").css("visibility", "hidden");
                        },
                        complete: function () {

                        }
                    });


                },
                complete: function () {
                }
            });

        }
    </script>
    <script type="text/javascript">
        function loadAttRow(epfno2) {
            $.ajax({
                url: "https://localhost:44341/api/ProcessSalary/CheckEmployee?epfno=" + epfno2 + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&empid=" + document.getElementById('text_attendanceId').value,

                success: function (data2) {
                    var empState = data2
                    $.ajax({
                        url: "https://localhost:44341/api/attendance/GetAttMonthRow?empname=" + epfno2 + "&year=" + document.getElementById('date_search').value + "&month=" + document.getElementById('list_month').value + "&empid=" + document.getElementById('text_attendanceId').value,

                        success: function (data) {
                            div_table.style.display = "block";
                            var table2 = $("#table2 tbody");
                            table2.empty();
                            function createDeleteButton() {
                                var deleteButton = $("<button>").text("Remove").attr("class", "btn btn-danger btn-rounded btn-sm");
                                deleteButton.click(function () {

                                    var value = $(this).closest("tr").find("td:first").text();
                                    if (confirm("Are you sure you want to delete this row?")) {
                                        $(this).closest("tr").remove();
                                        $.ajax({
                                            beforeSend: function () {
                                                $("#loading").css("visibility", "visible");
                                            },
                                            url: "https://localhost:44341/api/attendance/DeleteAtt?empname=" + epfno2 + "&dateTime=" + value + "&empid=" + document.getElementById('text_attendanceId').value,
                                            type: "DELETE",
                                            success: function (data) {
                                                $.ajax({
                                                    url: "https://localhost:44341/api/ProcessSalary/PushEmployeeDayDouble?epfno=" + epfno2 + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&requestFrom=Manual Attendance&user_=_&ip=" + value + "&empid=" + document.getElementById('text_attendanceId').value,
                                                    type: "PUT",
                                                    success: function (data) {
                                                        alert(value + " has been Deleted Successfully");
                                                        $("#loading").css("visibility", "hidden");
                                                    }
                                                });


                                            },
                                            complete: function () {
                                            }
                                        });
                                    }
                                });
                                return deleteButton;
                            }
                            if (empState == 1) {
                                $.each(data, function (index, value) {
                                    var row = $("<tr>");
                                    row.append($("<td>").text(value.DateTime));
                                    table2.append(row);
                                })
                            } else {
                                $.each(data, function (index, value) {
                                    var row = $("<tr>");
                                    row.append($("<td>").text(value.DateTime));
                                    row.append($("<td>").append(createDeleteButton()));
                                    table2.append(row);
                                });
                            }

                        },
                        complete: function () {
                        }
                    });
                },
                complete: function () {

                }
            });

        }
    </script>
    <script type="text/javascript">
        function loadLeaveBalance(epfno) {

            $.ajax({
                beforeSend: function () {
                    $("#loading").css("visibility", "visible");
                },
                url: "https://localhost:44341/api/Leave/getLeaveBalance?empname=" + epfno + "&year=" + document.getElementById('date_search').value + "&month=" + document.getElementById('list_month').value + "&empid=" + document.getElementById('text_attendanceId').value,
                beforeSend: function () {
                    $("#loading").css("visibility", "visible");
                },
                success: function (data) {
                    $.each(data, function (index, value) {
                        document.getElementById('text_totalLeave').value = value.TotalLeave + " Days"
                        document.getElementById('text_actualLeave').value = value.ActualLeave + " Days"
                        document.getElementById('text_balanceLeave').value = value.BalanceLeave + " Days"
                        document.getElementById('text_nopayDays').value = value.NoPayDays;
                        document.getElementById('text_workDays').value = value.WorkingDays;
                        document.getElementById('text_workHours').value = value.TotalWorkHours;
                        document.getElementById('text_paycut').value = value.TotalLate;
                        document.getElementById('text_ot').value = value.TotalOT;
                        document.getElementById('text_extraot').value = value.TotalExtraOT;

                        document.getElementById('text_totalLeave2').value = value.TotalLeave + " Days"
                        document.getElementById('text_actualLeave2').value = value.ActualLeave + " Days"
                        document.getElementById('text_balanceLeave2').value = value.BalanceLeave + " Days"
                        document.getElementById('text_WorkHours2').value = value.TotalWorkHours;
                        document.getElementById('text_PayCut2').value = value.TotalLate;
                        document.getElementById('text_Ot2').value = value.TotalOT;
                        document.getElementById('text_ExtraOt2').value = value.TotalExtraOT;
                    });
                },
                complete: function () {
                }
            });

        }
    </script>
    <script type="text/javascript">
        function chnageCheckBox(epfNo, date, columnName, status) {
            $.ajax({
                beforeSend: function () {
                    $("#loading").css("visibility", "visible");
                },
                url: "https://localhost:44341/api/roaster/updateRoasterRef?empname=" + epfNo + "&date=" + date + "&columnName=" + columnName + "&status=" + status + "&year=" + document.getElementById('date_search').value + "&empid=" + document.getElementById('text_attendanceId').value,
                type: "PUT",
                success: function (data) {
                    $.ajax({
                        url: "https://localhost:44341/api/ProcessSalary/PushEmployeeDay?epfno=" + epfNo + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&requestFrom=Advance Leave Update&user_=_&ip=" + date + "&empid=" + document.getElementById('text_attendanceId').value,
                        type: "PUT",
                        success: function (data) {
                        }
                    });

                    //  alert("Successfully Updated.");
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (xhr.status == 400) {
                        alert("Duplicate entry found for " + dateTime);
                    } else if (xhr.status == 404) {
                        alert("Leave Balance not Available.....");
                        $("#loading").css("visibility", "visible");
                        var table = $("#table tbody");
                        table.empty();
                        loadRoasterMonth(epfNo)
                    } else {
                        alert("Error adding record. Please try again." + xhr.status);
                    }
                },
                complete: function () {
                    loadLeaveBalance(epfNo)
                }
            });
        }
    </script>
    <script type="text/javascript">
        function chnageCheckBox2(epfNo, date, columnName, status) {
            $.ajax({
                beforeSend: function () {
                    $("#loading").css("visibility", "visible");
                },
                url: "https://localhost:44341/api/roaster/updateAttendanceCleaning?empname=" + epfNo + "&date=" + date + "&columnName=" + columnName + "&status=" + status + "&year=" + document.getElementById('date_search').value + "&empid=" + document.getElementById('text_attendanceId').value,
                type: "PUT",
                success: function (data) {
                    $.ajax({
                        url: "https://localhost:44341/api/ProcessSalary/PushEmployeeDay?epfno=" + epfNo + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&requestFrom=Advance Leave Update&user_=_&ip=" + date + "&empid=" + document.getElementById('text_attendanceId').value,
                        type: "PUT",
                        success: function (data) {
                        }
                    });

                    //  alert("Successfully Updated.");
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (xhr.status == 400) {
                        alert("Duplicate entry found for " + dateTime);
                    } else if (xhr.status == 404) {
                        alert("Leave Balance not Available.....");
                        $("#loading").css("visibility", "visible");
                        var table = $("#table tbody");
                        table.empty();
                        loadRoasterMonth(epfNo)
                    } else {
                        alert("Error adding record. Please try again." + xhr.status);
                    }
                },
                complete: function () {
                    loadLeaveBalance(epfNo)
                }
            });
        }
    </script>
</head>
<body>
    <!-- START PAGE CONTAINER -->
    <form runat="server">
        <div class="page-container">

            <!-- START PAGE SIDEBAR -->
            <div class="page-sidebar">
                <!-- START X-NAVIGATION -->
                <ul class="x-navigation">
                    <li class="xn-logo">
                        <a href="index.html">HRIS</a>
                        <a href="#" class="x-navigation-control"></a>
                    </li>
                    <li class="xn-profile">
                        <a href="#" class="profile-mini">
                            <img src="assets/images/users/avatar.png" alt="John Doe" />
                        </a>
                        <div class="profile">
                            <div class="profile-image">
                                <img src="assets/images/users/avatar.png" alt="John Doe" />
                            </div>
                            <div class="profile-data">
                                <div class="profile-data-name">Sadali Perera</div>
                                <div class="profile-data-title">HR Admin</div>
                            </div>
                            <div class="profile-controls">
                                <a href="pages-profile.html" class="profile-control-left"><span class="fa fa-info"></span></a>
                                <a href="pages-messages.html" class="profile-control-right"><span class="fa fa-envelope"></span></a>
                            </div>
                        </div>
                    </li>
                    <li>
                        <a href="home.aspx"><span class="fa fa-desktop"></span><span class="xn-text">Dashboard</span></a>
                    </li>
                    <li class="xn-openable">
                        <a href="#"><span class="fa fa-files-o"></span><span class="xn-text">Profiles</span></a>
                        <ul>
                            <li><a href="profile_employee.aspx"><span class="fa fa-image"></span>Employee Master</a></li>

                        </ul>
                    </li>
                    <li>
                        <a href="payroll.aspx"><span class="fa fa-file-text-o"></span><span class="xn-text">Payroll</span></a>

                    </li>
                    <li class="xn-openable active">
                        <a href="#"><span class="fa fa-file-text-o"></span><span class="xn-text">Time & Attendance</span></a>
                        <ul>
                            <li><a href="attendance.aspx"><span class="fa fa-heart"></span>Attendance</a></li>
                            <li class="active"><a href="roaster.aspx"><span class="fa fa-cogs"></span>Roaster</a></li>
                        </ul>
                    </li>
                    <li class="xn-openable">
                        <a href="#"><span class="fa fa-cogs"></span><span class="xn-text">On-Going Function</span></a>
                        <ul>
                            <li><a href="deduction.aspx"><span class="fa fa-heart"></span>Deductions & Allowances</a></li>
                            <li><a href="leave.aspx"><span class="fa fa-square-o"></span>Leave</a></li>

                        </ul>
                    </li>
                    <li class="xn-openable">
                        <a href="#"><span class="fa fa-pencil"></span><span class="xn-text">Reports</span></a>
                        <ul>
                            <li><a href="report_salary_summary.aspx"><span class="fa fa-file-text-o"></span>Salary Summary (Employee)</a></li>
                            <li><a href="advance_reports.aspx"><span class="fa fa-file-text-o"></span>Other Reports</a></li>
                        </ul>
                    </li>
                    <li class="xn-openable">
                        <a href="tables.html"><span class="fa fa-table"></span><span class="xn-text">Settings</span></a>
                        <ul>
                            <li><a href="settings_.aspx"><span class="fa fa-align-justify"></span>Settings</a></li>
                        </ul>
                    </li>



                </ul>
                <!-- END X-NAVIGATION -->
            </div>
            <!-- END PAGE SIDEBAR -->

            <!-- PAGE CONTENT -->
            <div class="page-content">

                <!-- START X-NAVIGATION VERTICAL -->
                <ul class="x-navigation x-navigation-horizontal x-navigation-panel">
                    <!-- TOGGLE NAVIGATION -->
                    <li class="xn-icon-button">
                        <a href="#" class="x-navigation-minimize"><span class="fa fa-dedent"></span></a>
                    </li>
                    <!-- END TOGGLE NAVIGATION -->
                    <!-- SEARCH -->
                    <li class="xn-search">
                        <form role="form">
                            <input type="text" name="search" placeholder="Search..." />
                        </form>
                    </li>
                    <!-- END SEARCH -->
                    <!-- SIGN OUT -->
                    <li class="xn-icon-button pull-right">
                        <a href="#" class="mb-control" data-box="#mb-signout"><span class="fa fa-sign-out"></span></a>
                    </li>
                    <!-- END SIGN OUT -->
                    <!-- MESSAGES -->
                    <li class="xn-icon-button pull-right">
                        <a href="#"><span class="fa fa-comments"></span></a>
                        <div class="informer informer-danger">4</div>
                        <div class="panel panel-primary animated zoomIn xn-drop-left xn-panel-dragging">
                            <div class="panel-heading">
                                <h3 class="panel-title"><span class="fa fa-comments"></span>Messages</h3>
                                <div class="pull-right">
                                    <span class="label label-danger">4 new</span>
                                </div>
                            </div>
                            <div class="panel-body list-group list-group-contacts scroll" style="height: 200px;">
                                <a href="#" class="list-group-item">
                                    <div class="list-group-status status-online"></div>
                                    <img src="assets/images/users/user2.jpg" class="pull-left" alt="John Doe" />
                                    <span class="contacts-title">John Doe</span>
                                    <p>Praesent placerat tellus id augue condimentum</p>
                                </a>
                                <a href="#" class="list-group-item">
                                    <div class="list-group-status status-away"></div>
                                    <img src="assets/images/users/user.jpg" class="pull-left" alt="Dmitry Ivaniuk" />
                                    <span class="contacts-title">Dmitry Ivaniuk</span>
                                    <p>Donec risus sapien, sagittis et magna quis</p>
                                </a>
                                <a href="#" class="list-group-item">
                                    <div class="list-group-status status-away"></div>
                                    <img src="assets/images/users/user3.jpg" class="pull-left" alt="Nadia Ali" />
                                    <span class="contacts-title">Nadia Ali</span>
                                    <p>Mauris vel eros ut nunc rhoncus cursus sed</p>
                                </a>
                                <a href="#" class="list-group-item">
                                    <div class="list-group-status status-offline"></div>
                                    <img src="assets/images/users/user6.jpg" class="pull-left" alt="Darth Vader" />
                                    <span class="contacts-title">Darth Vader</span>
                                    <p>I want my money back!</p>
                                </a>
                            </div>
                            <div class="panel-footer text-center">
                                <a href="pages-messages.html">Show all messages</a>
                            </div>
                        </div>
                    </li>
                    <!-- END MESSAGES -->
                    <!-- TASKS -->
                    <li class="xn-icon-button pull-right">
                        <a href="#"><span class="fa fa-tasks"></span></a>
                        <div class="informer informer-warning">3</div>
                        <div class="panel panel-primary animated zoomIn xn-drop-left xn-panel-dragging">
                            <div class="panel-heading">
                                <h3 class="panel-title"><span class="fa fa-tasks"></span>Tasks</h3>
                                <div class="pull-right">
                                    <span class="label label-warning">3 active</span>
                                </div>
                            </div>
                            <div class="panel-body list-group scroll" style="height: 200px;">
                                <a class="list-group-item" href="#">
                                    <strong>Phasellus augue arcu, elementum</strong>
                                    <div class="progress progress-small progress-striped active">
                                        <div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100" style="width: 50%;">50%</div>
                                    </div>
                                    <small class="text-muted">John Doe, 25 Sep 2014 / 50%</small>
                                </a>
                                <a class="list-group-item" href="#">
                                    <strong>Aenean ac cursus</strong>
                                    <div class="progress progress-small progress-striped active">
                                        <div class="progress-bar progress-bar-warning" role="progressbar" aria-valuenow="80" aria-valuemin="0" aria-valuemax="100" style="width: 80%;">80%</div>
                                    </div>
                                    <small class="text-muted">Dmitry Ivaniuk, 24 Sep 2014 / 80%</small>
                                </a>
                                <a class="list-group-item" href="#">
                                    <strong>Lorem ipsum dolor</strong>
                                    <div class="progress progress-small progress-striped active">
                                        <div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="95" aria-valuemin="0" aria-valuemax="100" style="width: 95%;">95%</div>
                                    </div>
                                    <small class="text-muted">John Doe, 23 Sep 2014 / 95%</small>
                                </a>
                                <a class="list-group-item" href="#">
                                    <strong>Cras suscipit ac quam at tincidunt.</strong>
                                    <div class="progress progress-small">
                                        <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%;">100%</div>
                                    </div>
                                    <small class="text-muted">John Doe, 21 Sep 2014 /</small><small class="text-success"> Done</small>
                                </a>
                            </div>
                            <div class="panel-footer text-center">
                                <a href="pages-tasks.html">Show all tasks</a>
                            </div>
                        </div>
                    </li>
                    <!-- END TASKS -->
                </ul>
                <!-- END X-NAVIGATION VERTICAL -->

                <!-- START BREADCRUMB -->
                <ul class="breadcrumb">
                    <li><a href="home.aspx">Dashboard</a></li>
                    <li class="active">Roaster</li>
                </ul>
                <!-- END BREADCRUMB -->

                <!-- PAGE CONTENT WRAPPER -->
                <div class="page-content-wrap">
                    <div class="row">
                        <div class="col-md-12">

                            <div class="block">

                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <asp:DropDownList runat="server" ID="date_search" CssClass="form-control" Width="100px" onchange="myFunctionDate()">
                                        </asp:DropDownList>
                                        <script>
                                            function myFunctionDate() {
                                                if (document.getElementById('text_epfNo_').value == "") {


                                                } else {
                                                    var table = $("#table tbody");
                                                    table.empty();
                                                    var data = document.getElementById('text_epfNo_').value
                                                    loadRoasterMonth(data)
                                                }
                                            }
                                        </script>

                                    </div>

                                </div>
                                <div class="col-md-2">
                                    <div class="form-group has-success">

                                        <select class="form-control select" data-style="btn-success" id="list_month" onchange="myFunction()" runat="server">
                                            <option>January</option>
                                            <option>February</option>
                                            <option>March</option>
                                            <option>April</option>
                                            <option>May</option>
                                            <option>June</option>
                                            <option>July</option>
                                            <option>August</option>
                                            <option>September</option>
                                            <option>Octomber</option>
                                            <option>November</option>
                                            <option>December</option>
                                        </select>
                                        <script>
                                            function myFunction() {
                                                if (document.getElementById('text_epfNo_').value == "") {

                                                } else {

                                                    var table = $("#table tbody");
                                                    table.empty();
                                                    var data = document.getElementById('text_epfNo_').value
                                                    loadRoasterMonth(data)
                                                }
                                            }
                                        </script>
                                    </div>

                                </div>
                                <div class="col-md-1">
                                    <asp:DropDownList CssClass="form-control" ID="list_lines" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>

                                </div>
                                <div class="col-md-4">
                                    <div class="form-group has-success">
                                        <asp:TextBox ID="text_employee" runat="server" CssClass="form-control" Text="" Placeholder="Search Employee here...." />
                                    </div>

                                </div>

                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" id="label_status" runat="server"></label>

                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" id="label_status2" runat="server"></label>

                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="block">

                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label">EPF No</label>
                                        <input type="text" class="form-control" value="" id="text_epfNo_" runat="server" readonly style="color: Green" />
                                    </div>

                                </div>
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label">Employee No</label>
                                        <input type="text" class="form-control" value="" id="text_attendanceId" runat="server" readonly style="color: Green" />
                                    </div>

                                </div>
                                <div class="col-md-3">
                                    <div class="form-group has-success">
                                        <label class="control-label">Employee Name</label>
                                        <input type="text" class="form-control" value="" id="text_employeeName" runat="server" readonly style="color: Green" />
                                    </div>

                                </div>
                                <div class="col-md-2">
                                    <div class="form-group has-success">
                                        <label class="control-label">Department</label>
                                        <input type="text" class="form-control" value="" id="text_department" runat="server" readonly style="color: Green" />
                                    </div>

                                </div>
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label">Working Days</label>
                                        <input type="text" class="form-control" value="" id="text_workDays" runat="server" readonly style="color: Green" />
                                    </div>

                                </div>
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label">Nopay Days</label>
                                        <input type="text" class="form-control" value="" id="text_nopayDays" runat="server" readonly style="color: Green" />
                                    </div>

                                </div>
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label">.</label>
                                        <asp:Button ID="Refresh" runat="server" Text="Refresh" CssClass="form-control" BackColor="#0099ff" ForeColor="White" Font-Bold="true"></asp:Button>
                                        <script>
                                            const button5 = document.querySelector("#Refresh");

                                            button5.addEventListener("click", function () {
                                                myMethod5();
                                                event.preventDefault();
                                            });

                                            function myMethod5() {

                                                $.ajax({

                                                    type: "POST",
                                                    url: "roaster.aspx/employeeSelected",
                                                    data: "{'empName':'" + document.getElementById('text_epfNo_').value + "','year':'" + document.getElementById('date_search').value + "','month':'" + document.getElementById('list_month').value + "'}",
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (data) {
                                                        var table = $("#table tbody");
                                                        table.empty();
                                                        if (data.d == '') {
                                                            document.getElementById('text_employee').value = ""
                                                            document.getElementById('text_epfNo_').value = ""
                                                            document.getElementById('text_attendanceId').value = ""
                                                            document.getElementById('text_employeeName').value = ""
                                                            document.getElementById('text_department').value = ""
                                                            document.getElementById('text_workDays').value = ""
                                                            document.getElementById('text_nopayDays').value = ""

                                                            loadLeaveBalance(data)
                                                        } else {
                                                            var result = data.d.split('_')
                                                            document.getElementById('text_employee').value = ""
                                                            document.getElementById('text_epfNo_').value = result[0]
                                                            document.getElementById('text_attendanceId').value = result[1]
                                                            document.getElementById('text_employeeName').value = result[2]
                                                            document.getElementById('text_department').value = result[3]
                                                            loadRoasterMonth(result[0])
                                                        }
                                                    },
                                                    error: function (result) {


                                                    }
                                                });

                                            }
                                        </script>
                                    </div>

                                </div>
                                <div class="col-md-2">
                                    <div class="form-group has-success">
                                        <label class="control-label">.</label>
                                        <asp:Button ID="btn_processRequest" runat="server" Text="Full Process Request" CssClass="form-control" BackColor="#333333" ForeColor="White" Font-Bold="true"></asp:Button>
                                        <script>
                                            const button6 = document.querySelector("#btn_processRequest");

                                            button6.addEventListener("click", function () {
                                                myMethod6();
                                                event.preventDefault();
                                            });

                                            function myMethod6() {

                                                $.ajax({
                                                    beforeSend: function () {
                                                        $("#loading").css("visibility", "visible");
                                                    },
                                                    url: "https://localhost:44341/api/ProcessSalary/PushEmployee?epfno=" + document.getElementById('text_epfNo_').value + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&requestFrom=_&user_=_&ip=_" + "&empid=" + document.getElementById('text_attendanceId').value,
                                                    type: "PUT",
                                                    success: function (data) {
                                                    },
                                                    complete: function () {
                                                        $("#loading").css("visibility", "hidden");
                                                    }
                                                });
                                            }
                                        </script>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <panel class="form-horizontal">

                                <div class="panel panel-default tabs">
                                    <ul class="nav nav-tabs" role="tablist">
                                        <li class="active"><a href="#tab-first" role="tab" data-toggle="tab">Roaster | Time Sheet</a></li>
                                        <li><a href="#tab-second" role="tab" data-toggle="tab">Row Attendance | Manual Entry</a></li>
                                        <li><a href="#tab-third" role="tab" data-toggle="tab">Summary | Requests</a></li>
                                        <li id="li_four" runat="server"><a href="#tab-four" role="tab" data-toggle="tab">Advance Manual Entry | Attendance</a></li>
                                    </ul>
                                    <div class="panel-body tab-content">
                                        <div class="tab-pane active" id="tab-first">
                                            <div class="row">
                                                <table class="table" id="table">
                                                    <thead>
                                                        <tr>

                                                            <th style="width: 10%">In Date</th>
                                                            <th>Frist Shift</th>
                                                            <th>In Time</th>
                                                            <th>Out Time</th>
                                                            <th>Second Shift</th>
                                                            <th>In Time</th>
                                                            <th>Out Time</th>
                                                            <th>Work Hours</th>
                                                            <th>Pay Cut</th>
                                                            <th>OT 1.5</th>
                                                            <th style="width: 15%">Extra OT</th>
                                                            <th>D/O</th>
                                                            <th>Leave</th>
                                                            <th>Leave Half</th>
                                                            <th>Sleep /D</th>
                                                            <th>Leave /Leave</th>
                                                            <th>Multi Shift</th>
                                                            <th>Pre OT</th>
                                                            <th>No Pay</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody></tbody>
                                                </table>
                                            </div>
                                            <div class="col-md-3">


                                                <panel class="form-horizontal">

                                                    <div class="panel panel-default">
                                                        <div class="panel-body ">
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Total Leave</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_totalLeave2" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Actual Leave</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_actualLeave2" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Balance Leave</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_balanceLeave2" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </panel>
                                            </div>
                                            <div class="col-md-3">


                                                <panel class="form-horizontal">

                                                    <div class="panel panel-default">
                                                        <div class="panel-body ">
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Total WorkHours</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_WorkHours2" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Total PayCut</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_PayCut2" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Total OT</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_Ot2" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Total Extra OT</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_ExtraOt2" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </panel>
                                            </div>
                                        </div>
                                        <div class="tab-pane" id="tab-second">
                                            <div class="row">
                                                <div class="col-md-3">


                                                    <panel class="form-horizontal">

                                                        <div class="panel panel-default">
                                                            <div class="panel-body ">
                                                                <div class="panel-body ">


                                                                    <div class="form-group has-success" style="width: 80%; vertical-align: central">
                                                                        <input type="text" class="form-control datepicker" value="" placeholder="select date here" id="date_manual" runat="server">
                                                                        <input type="text" class="form-control timepicker24" id="time_manual" runat="server" />
                                                                        <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span></span>
                                                                    </div>

                                                                    <br />
                                                                    <div class="form-group bootstro-next-btn">
                                                                        <input type="button" class="form-control" value="Save Record" onclick="addTime()" style="background: #ff6a00; color: white">
                                                                        <script type="text/javascript">
                                                                            function addTime() {

                                                                                var empname = document.getElementById('text_epfNo_').value;
                                                                                var dateTime = document.getElementById('date_manual').value + " " + document.getElementById('time_manual').value;


                                                                                if (empname != "") {
                                                                                    $.ajax({
                                                                                        beforeSend: function () {
                                                                                            $("#loading").css("visibility", "visible");
                                                                                        },
                                                                                        url: "https://localhost:44341/api/ProcessSalary/CheckEmployee?epfno=" + empname + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&empid=" + document.getElementById('text_attendanceId').value,

                                                                                        success: function (data) {
                                                                                            if (data != 1) {
                                                                                                $.ajax({

                                                                                                    url: "https://localhost:44341/api/attendance/AddAtt?empname=" + empname + "&dateTime=" + dateTime + "&empid=" + document.getElementById('text_attendanceId').value,
                                                                                                    type: "POST",
                                                                                                    success: function (data) {
                                                                                                        loadAttRow(empname);
                                                                                                        $.ajax({
                                                                                                            url: "https://localhost:44341/api/ProcessSalary/PushEmployeeDayDouble?epfno=" + empname + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&requestFrom=Manual Attendance&user_=_&ip=" + dateTime + "&empid=" + document.getElementById('text_attendanceId').value,
                                                                                                            type: "PUT",
                                                                                                            success: function (data) {
                                                                                                                loadAttRow(empname);
                                                                                                                alert(dateTime + " has been Successfully Added.");

                                                                                                            }
                                                                                                        });

                                                                                                    },
                                                                                                    error: function (xhr, textStatus, errorThrown) {
                                                                                                        if (xhr.status == 400) {
                                                                                                            alert("Duplicate entry found for " + dateTime);
                                                                                                        } else {
                                                                                                            alert("Error adding record. Please try again." + xhr.status);
                                                                                                        }
                                                                                                    },
                                                                                                    complete: function () {
                                                                                                        $("#loading").css("visibility", "hidden");
                                                                                                    }
                                                                                                });
                                                                                            } else if (data == 1) {
                                                                                                alert("Sorry,Selected Employee Locked in " + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value)

                                                                                            }

                                                                                        },
                                                                                        complete: function () {
                                                                                            $("#loading").css("visibility", "hidden");
                                                                                        }
                                                                                    });

                                                                                }
                                                                                else {
                                                                                    alert("Please Select Employee First")
                                                                                }

                                                                            }

                                                                        </script>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </panel>
                                                </div>
                                                <div class="col-md-3">

                                                    <div class="table-responsive" style="display: none" id="div_table">
                                                        <table class="table table-bordered table-striped table-actions" id="table2" runat="server">
                                                            <thead>
                                                                <tr>

                                                                    <th>Date Time</th>

                                                                    <th width="100">actions</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                            </tbody>
                                                        </table>
                                                    </div>

                                                </div>
                                            </div>



                                        </div>
                                        <div class="tab-pane" id="tab-third">
                                            <div class="col-md-3">


                                                <panel class="form-horizontal">

                                                    <div class="panel panel-default">
                                                        <div class="panel-body ">
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Total Leave</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_totalLeave" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Actual Leave</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_actualLeave" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Balance Leave</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_balanceLeave" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </panel>
                                            </div>
                                            <div class="col-md-3">


                                                <panel class="form-horizontal">

                                                    <div class="panel panel-default">
                                                        <div class="panel-body ">
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Total WorkHours</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_workHours" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Total PayCut</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_paycut" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Total OT</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_ot" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group has-warning">
                                                                <label class="col-md-4 col-xs-12 control-label">Total Extra OT</label>
                                                                <div class="col-md-6 col-xs-12">
                                                                    <input type="text" class="form-control" value="" id="text_extraot" runat="server" readonly style="color: green" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </panel>
                                            </div>
                                        </div>
                                        <div class="tab-pane" id="tab-four">
                                            <div class="row">
                                                <table class="table" id="table4" style="width: 300px">
                                                    <thead>
                                                        <tr>

                                                            <th>In Date</th>
                                                            <th>SHIFT ONE</th>
                                                            <th>SHIFT TWO</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody></tbody>
                                                </table>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </panel>
                        </div>

                    </div>

                </div>



            </div>

            <!-- END PAGE CONTENT WRAPPER -->
        </div>
        <!-- END PAGE CONTENT -->

        <!-- MESSAGE BOX-->
        <div class="message-box animated fadeIn" data-sound="alert" id="mb-signout">
            <div class="mb-container">
                <div class="mb-middle">
                    <div class="mb-title"><span class="fa fa-sign-out"></span>Log <strong>Out</strong> ?</div>
                    <div class="mb-content">
                        <p>Are you sure you want to log out?</p>
                        <p>Press No if youwant to continue work. Press Yes to logout current user.</p>
                    </div>
                    <div class="mb-footer">
                        <div class="pull-right">
                            <a href="pages-login.html" class="btn btn-success btn-lg">Yes</a>
                            <button class="btn btn-default btn-lg mb-control-close">No</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="loading" runat="server">
            <img src="images/Loading_2.gif" alt="Loading...">
        </div>
        <!-- END MESSAGE BOX-->

        <!-- START PRELOADS -->
        <audio id="audio-alert" src="audio/alert.mp3" preload="auto"></audio>
        <audio id="audio-fail" src="audio/fail.mp3" preload="auto"></audio>
        <!-- END PRELOADS -->

        <!-- THIS PAGE PLUGINS -->
        <script type='text/javascript' src='js/plugins/icheck/icheck.min.js'></script>
        <script type="text/javascript" src="js/plugins/mcustomscrollbar/jquery.mCustomScrollbar.min.js"></script>

        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap-datepicker.js"></script>
        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap-timepicker.min.js"></script>
        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap-colorpicker.js"></script>
        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap-file-input.js"></script>
        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap-select.js"></script>
        <script type="text/javascript" src="js/plugins/tagsinput/jquery.tagsinput.min.js"></script>
        <!-- END THIS PAGE PLUGINS -->


        <!-- START TEMPLATE -->
        <script type="text/javascript" src="js/settings.js"></script>
        <script>
            nload = function () {

                var theDate = new Date();

                var inputDate = new wijmo.input.InputDate('#theInputDate', {
                    min: new Date(2014, 8, 1),
                    format: 'yyyy',
                    selectionMode: 2,
                    value: theDate,
                    isDroppedDownChanged: function (s, e) {
                        setTimeout(function () {
                            var _hdr = s.dropDown.querySelector(".wj-calendar-year tr.wj-header td");
                            console.log(_hdr)
                            _hdr.click();
                        }, 100)
                    }
                });
            }
        </script>
        <script type="text/javascript" src="js/plugins.js"></script>
        <script type="text/javascript" src="js/actions.js"></script>
    </form>
</body>
</html>
