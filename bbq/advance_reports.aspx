<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="advance_reports.aspx.cs" Inherits="bbq.advance_reports" EnableEventValidation="false" %>



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

    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="js/bootstrap-min.js"></script>

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">

    <script type="text/javascript">

        function loadPeriodList() {
            $.ajax({
                beforeSend: function () {
                    $("#loading").css("visibility", "visible");
                },
                url: "https://localhost:44341/api/SalaryLock/getPeriodListAll",
                success: function (data) {
                    var dropDownList = document.getElementById('<%= list_period.ClientID %>');
                    dropDownList.innerHTML = '';
                    var dropDownList2 = document.getElementById('<%= list_period2.ClientID %>');
                    dropDownList2.innerHTML = '';
                    var dropDownList3 = document.getElementById('<%= list_period3.ClientID %>');
                    dropDownList3.innerHTML = '';
                    $.each(data, function (index, value) {

                        var option = document.createElement('option');
                        option.text = value.Period;
                        option.value = value.Period;
                        dropDownList.add(option);

                    });
                    $.each(data, function (index, value) {

                        var option2 = document.createElement('option');
                        option2.text = value.Period;
                        option2.value = value.Period;
                        dropDownList2.add(option2);

                    });
                    $.each(data, function (index, value) {

                        var option3 = document.createElement('option');
                        option3.text = value.Period;
                        option3.value = value.Period;
                        dropDownList3.add(option3);

                    });
                },
                complete: function () {
                    $("#loading").css("visibility", "hidden");
                }
            });
        }

        function loadLockHistory() {
            $.ajax({
                beforeSend: function () {
                    $("#loading").css("visibility", "visible");
                },
                url: "https://localhost:44341/api/SalaryLock/getLockHistory",
                success: function (data) {
                    var table = $("#table tbody");
                    table.empty();
                    $.each(data, function (index, value) {
                        var row = $("<tr>");
                        row.append($("<td>").text(value.Period));
                        row.append($("<td>").text(value.DateTimeLock));

                        if (value.SmsSend) {
                            var statusColumn = $("<td>").text("SMS Sent @ " + value.SmsRequested);
                            statusColumn.addClass("label label-success"); // Add the class to the second column
                            row.append(statusColumn);
                        } else {
                            var statusColumn = $("<td>").text("Pending SMS Salary");
                            statusColumn.addClass("label label-danger"); // Add the class to the second column
                            row.append(statusColumn);

                            var checkbox = $("<input>").attr(
                                {
                                    type: "checkbox",
                                    checked: false
                                }
                            )
                            var checkboxCell = $("<td>").append(checkbox);
                            row.append(checkboxCell);
                            checkbox.change(function () {

                                var period = $(this).closest("tr").find("td:first").text();

                                $.ajax({
                                    beforeSend: function () {
                                        $("#loading").css("visibility", "visible");
                                    },
                                    url: "https://localhost:44341/api/SalaryLock/SmsSalaryRequest?period_=" + period,
                                    success: function (data) {
                                        loadLockHistory();
                                        alert("Salary SMS Requested Successfully for " + period);
                                    },
                                    complete: function () {
                                        $("#loading").css("visibility", "hidden");
                                    }
                                });
                            });
                        }

                        table.append(row);
                    });

                },
                complete: function () {
                    $("#loading").css("visibility", "hidden");
                }
            });
        }
        window.onload = function () {
            loadPeriodList();
            loadLockHistory();
        };
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
                            <li><a href="profile_users.aspx"><span class="fa fa-user"></span>Users</a></li>

                        </ul>
                    </li>
                    <li>
                        <a href="payroll.aspx"><span class="fa fa-file-text-o"></span><span class="xn-text">Payroll</span></a>

                    </li>
                    <li class="xn-openable">
                        <a href="#"><span class="fa fa-file-text-o"></span><span class="xn-text">Time & Attendance</span></a>
                        <ul>
                            <li><a href="attendance.aspx"><span class="fa fa-heart"></span>Attendance</a></li>
                            <li><a href="roaster.aspx"><span class="fa fa-cogs"></span>Roaster</a></li>
                        </ul>
                    </li>
                    <li class="xn-openable">
                        <a href="#"><span class="fa fa-cogs"></span><span class="xn-text">On-Going Function</span></a>
                        <ul>
                            <li><a href="deduction.aspx"><span class="fa fa-heart"></span>Deductions & Allowances</a></li>
                            <li><a href="leave.aspx"><span class="fa fa-square-o"></span>Leave</a></li>

                        </ul>
                    </li>
                    <li class="xn-openable active">
                        <a href="#"><span class="fa fa-pencil"></span><span class="xn-text">Reports</span></a>
                        <ul>
                            <li><a href="report_salary_summary.aspx"><span class="fa fa-file-text-o"></span>Salary Summary (Employee)</a></li>
                            <li class="active"><a href="advance_reports.aspx"><span class="fa fa-file-text-o"></span>Other Reports</a></li>
                        </ul>
                    </li>
                    <li class="xn-openable ">
                        <a href="#"><span class="fa fa-table"></span><span class="xn-text">Settings</span></a>
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
                    <li class="active">Other Reports</li>
                </ul>
                <!-- END BREADCRUMB -->

                <!-- PAGE CONTENT WRAPPER -->
                <div class="page-content-wrap">
                    <div class="row">
                        <div class="col-md-12">

                            <div class="block">

                                <div class="col-md-3">
                                    <div class="panel panel-default">

                                        <div class="panel-heading">
                                            <h3 class="panel-title">OT / Work hour Summary</h3>
                                        </div>
                                        <br />
                                        <hr width="0%" color="red" align="center">
                                        <div class="panel-body ">
                                            <div class="form-group has-success">
                                                <asp:DropDownList CssClass="form-control" ID="list_typeReportOT" runat="server" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group has-success">
                                                <asp:DropDownList CssClass="form-control" ID="list_period" runat="server" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group has-success">
                                                <input type="button" class="btn btn-primary pull-right" value="Export Report" onclick="lockSalary()" style="background: #ff6a00; color: white">
                                                <script type="text/javascript">
                                                    function lockSalary() {
                                                        $.ajax({
                                                            beforeSend: function () {
                                                                $("#loading").css("visibility", "visible");
                                                            },
                                                            url: "https://localhost:44341/api/ProcessSalary/CheckEmployeeAll?period_=" + document.getElementById('list_period').value,

                                                            success: function (data) {
                                                                if (data != 2) {
                                                                    var url = '';
                                                                    if (document.getElementById('list_typeReportOT').value == 'OverTime ( Selected Month )') {
                                                                        url = "https://localhost:44341/api/Export/GetOTAll?period=" + document.getElementById('list_period').value
                                                                    } else if (document.getElementById('list_typeReportOT').value == 'OverTime ( Audit )') {
                                                                        url = "https://localhost:44341/api/Export/GetOTAllAudit?period=" + document.getElementById('list_period').value
                                                                    } else if (document.getElementById('list_typeReportOT').value == 'WorkHours ( Selected Month )') {
                                                                        url = "https://localhost:44341/api/Export/GetWorkHours?period=" + document.getElementById('list_period').value
                                                                    } else if (document.getElementById('list_typeReportOT').value == 'WorkHours ( Audit )') {
                                                                        url = "https://localhost:44341/api/Export/GetWorkHoursAudit?period=" + document.getElementById('list_period').value
                                                                    }
                                                                    $.ajax({
                                                                        beforeSend: function () {
                                                                            $("#loading").css("visibility", "visible");
                                                                        },
                                                                        url: url,
                                                                        xhrFields: {
                                                                            responseType: 'blob' // Set the response type to 'blob'
                                                                        },
                                                                        success: function (data, status, xhr) {
                                                                            var url = URL.createObjectURL(data);
                                                                            var newTab = window.open(url, '_blank');
                                                                            if (newTab === null || typeof newTab === 'undefined') {
                                                                                alert("Failed to open PDF in a new tab. Please check your browser settings.");
                                                                            }

                                                                            URL.revokeObjectURL(url);
                                                                        },
                                                                        complete: function () {
                                                                            $("#loading").css("visibility", "hidden");
                                                                        }
                                                                    });

                                                                } else if (data == 2) {
                                                                    alert("Selected Period Processing......Cann't Export")
                                                                    $("#loading").css("visibility", "hidden");
                                                                }

                                                            },
                                                            complete: function () {

                                                            }
                                                        });


                                                    }

                                                </script>
                                            </div>

                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="panel panel-default">

                                        <div class="panel-heading">
                                            <h3 class="panel-title">Attendance Based Summary</h3>
                                        </div>
                                        <hr width="0%" color="red" align="center">
                                        <div class="panel-body ">
                                            <div class="form-group has-success">
                                                <div class="form-group has-success">
                                                    <select class="form-control" id="list_type2" runat="server">
                                                        <option>No Pay Report</option>
                                                        <option>No Pay Report ( Detail )</option>
                                                    </select>
                                                </div>

                                            </div>
                                            <div class="form-group has-success">
                                                <asp:DropDownList CssClass="form-control" ID="list_period2" runat="server" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>

                                            <div>
                                                <asp:Button ID="btn_save" class="btn btn-primary pull-right" Text="Export Report" runat="server" />

                                                <script type="text/javascript">
                                                    const button2 = document.querySelector("#btn_save");

                                                    button2.addEventListener("click", function () {
                                                        invokeJavaScript();
                                                        event.preventDefault();
                                                    });
                                                    function invokeJavaScript() {

                                                        $.ajax({
                                                            beforeSend: function () {
                                                                $("#loading").css("visibility", "visible");
                                                            },
                                                            url: "https://localhost:44341/api/ProcessSalary/CheckEmployeeAll?period_=" + document.getElementById('list_period2').value,

                                                            success: function (data) {
                                                                if (data !== 2) {
                                                                    var url = '';
                                                                    if (document.getElementById('list_type2').value == 'No Pay Report') {
                                                                        url = "https://localhost:44341/api/Export/GetNoPaySummary?period=" + document.getElementById('list_period2').value
                                                                    } else if (document.getElementById('list_type2').value == 'No Pay Report ( Detail )') {
                                                                        url = "https://localhost:44341/api/Export/GetNoPaySummaryDetail?period=" + document.getElementById('list_period2').value
                                                                    }
                                                                    $.ajax({
                                                                        beforeSend: function () {
                                                                            $("#loading").css("visibility", "visible");
                                                                        },
                                                                        url: url,
                                                                        xhrFields: {
                                                                            responseType: 'blob' // Set the response type to 'blob'
                                                                        },
                                                                        success: function (data, status, xhr) {
                                                                            var url = URL.createObjectURL(data);
                                                                            var newTab = window.open(url, '_blank');
                                                                            if (newTab === null || typeof newTab === 'undefined') {
                                                                                alert("Failed to open PDF in a new tab. Please check your browser settings.");
                                                                            }

                                                                            URL.revokeObjectURL(url);
                                                                        },
                                                                        complete: function () {
                                                                            $("#loading").css("visibility", "hidden");
                                                                        }
                                                                    });

                                                                } else if (data == 2) {
                                                                    alert("Selected Period Processing......Cann't Export")
                                                                    $("#loading").css("visibility", "hidden");
                                                                }

                                                            },
                                                            complete: function () {

                                                            }
                                                        });


                                                    }

                                                </script>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="panel panel-default">

                                        <div class="panel-heading">
                                            <h3 class="panel-title">Dedcutions | Allowances </h3>
                                        </div>

                                        <div class="panel-body ">
                                            <div class="form-group has-success">
                                                <div class="form-group has-success">
                                                    <select class="form-control" id="list_type3" runat="server">
                                                        <option>Deduction</option>
                                                        <option>Allowances</option>
                                                    </select>
                                                </div>

                                            </div>
                                            <div class="form-group has-success">
                                                <asp:DropDownList CssClass="form-control" ID="list_period3" runat="server" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                        <div>
                                            <asp:Button ID="btn_deductionLoad" class="btn btn-primary pull-right" Text="Export Report" runat="server" BackColor="#0099ff" />
                                            <script type="text/javascript">
                                                const button3 = document.querySelector("#btn_deductionLoad");

                                                button3.addEventListener("click", function () {
                                                    invokeJavaScript2();
                                                    event.preventDefault();
                                                });
                                                function invokeJavaScript2() {
                                                    $.ajax({
                                                        beforeSend: function () {
                                                            $("#loading").css("visibility", "visible");
                                                        },
                                                        url: "https://localhost:44341/api/ProcessSalary/CheckEmployeeAll?period_=" + document.getElementById('list_period3').value,

                                                        success: function (data) {
                                                            if (true) {
                                                                var url = '';
                                                                if (document.getElementById('list_type3').value == 'Deduction') {
                                                                    url = "https://localhost:44341/api/Export/GetDeductionSummary?period=" + document.getElementById('list_period3').value
                                                                } else if (document.getElementById('list_type3').value == 'Allowances') {
                                                                    url = "https://localhost:44341/api/Export/GetAllowanceSummary?period=" + document.getElementById('list_period3').value
                                                                }
                                                                $.ajax({
                                                                    beforeSend: function () {
                                                                        $("#loading").css("visibility", "visible");
                                                                    },
                                                                    url: url,
                                                                    xhrFields: {
                                                                        responseType: 'blob' // Set the response type to 'blob'
                                                                    },
                                                                    success: function (data, status, xhr) {
                                                                        var url = URL.createObjectURL(data);
                                                                        var newTab = window.open(url, '_blank');
                                                                        if (newTab === null || typeof newTab === 'undefined') {
                                                                            alert("Failed to open PDF in a new tab. Please check your browser settings.");
                                                                        }

                                                                        URL.revokeObjectURL(url);
                                                                    },
                                                                    complete: function () {
                                                                        $("#loading").css("visibility", "hidden");
                                                                    }
                                                                });

                                                            } else if (data == 2) {
                                                                alert("Selected Period Processing......Cann't Export")
                                                                $("#loading").css("visibility", "hidden");
                                                            }

                                                        },
                                                        complete: function () {

                                                        }
                                                    });
                                                }

                                            </script>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

            </div>

            <!-- END PAGE CONTENT WRAPPER -->
        </div>
        <!-- END PAGE CONTENT -->
        </div>
        <div id="loading" runat="server">
            <img src="images/Loading_2.gif" alt="Loading...">
        </div>
        <!-- END PAGE CONTAINER -->

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


        <script type="text/javascript" src="js/plugins.js"></script>
        <script type="text/javascript" src="js/actions.js"></script>

        <!-- START TEMPLATE -->
        <script type="text/javascript" src="js/settings.js"></script>
        <script>
            nload = function () {

                // the date/time being edited
                var theDate = new Date();

                // create InputDate control
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

    </form>
</body>
</html>

