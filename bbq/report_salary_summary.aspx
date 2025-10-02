<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report_salary_summary.aspx.cs" Inherits="bbq.report_salary_summary" %>


<!DOCTYPE html>

<html lang="en">
<head>
    <title>HRIS</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="icon" href="favicon.ico" type="image/x-icon" />
    <!-- END META SECTION -->

    <!-- CSS INCLUDE -->
    <link href="css/styleloading.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" id="theme" href="css/theme-default.css" />
    <!-- EOF CSS INCLUDE -->
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
                            <img src="assets/images/users/avatar.jpg" alt="John Doe" />
                        </a>
                        <div class="profile">
                            <div class="profile-image">
                                <img src="assets/images/users/avatar.jpg" alt="John Doe" />
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
                    <li class="xn-openable ">
                        <a href="#"><span class="fa fa-file-text-o"></span><span class="xn-text">Time & Attendance</span></a>
                        <ul>
                            <li><a href="attendance.aspx"><span class="fa fa-heart"></span>Attendance</a></li>
                            <li><a href="roaster.aspx"><span class="fa fa-cogs"></span>Roaster</a></li>
                        </ul>
                    </li>
                    <li class="xn-openable ">
                        <a href="#"><span class="fa fa-cogs"></span><span class="xn-text">On-Going Function</span></a>
                        <ul>
                            <li><a href="deduction.aspx"><span class="fa fa-heart"></span>Deductions & Allowances</a></li>
                            <li><a href="leave.aspx"><span class="fa fa-square-o"></span>Leave</a></li>

                        </ul>
                    </li>
                    <li class="xn-openable active">
                        <a href="#"><span class="fa fa-pencil"></span><span class="xn-text">Reports</span></a>
                        <ul>
                            <li class="active"><a href="report_salary_summary.aspx"><span class="fa fa-file-text-o"></span>Salary Summary (Employee)</a></li>
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
                    <li class="active">Salary Summary</li>
                </ul>
                <!-- END BREADCRUMB -->

                <!-- PAGE CONTENT WRAPPER -->
                <div class="page-content-wrap">

                    <div class="row">
                        <div class="col-md-12">

                            <div class="block">

                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <asp:DropDownList runat="server" ID="list_date" CssClass="form-control" Width="100px" onchange="loadSummaryDate()">
                                        </asp:DropDownList>
                                        <script type="text/javascript">
                                            function loadSummaryDate() {

                                                $.ajax({
                                                    beforeSend: function () {
                                                        $("#loading").css("visibility", "visible");
                                                    },
                                                    url: "https://localhost:44341/api/Payroll/GetSummaryMonth?year=" + document.getElementById('list_date').value + "&month=" + document.getElementById('list_month').value + "&searchmode=" + document.getElementById('list_mode').value,
                                                    success: function (data) {
                                                        var table = $("#customers2 tbody");
                                                        table.empty();
                                                        $.each(data, function (index, value) {
                                                            var row = $("<tr>");
                                                            row.append($("<td>").text(value.Index));
                                                            row.append($("<td>").text(value.EpfNo));
                                                            row.append($("<td>").text(value.Name));
                                                            row.append($("<td>").text(value.Line));
                                                            row.append($("<td>").text(value.Basic));
                                                            row.append($("<td>").text(value.Budgatry));
                                                            row.append($("<td>").text(value.PHAllowance));
                                                            row.append($("<td>").text(value.Late));
                                                            row.append($("<td>").text(value.NoPay));
                                                            row.append($("<td>").text(value.TotalForEPF));
                                                            row.append($("<td>").text(value.TotalOtherAllowance));
                                                            row.append($("<td>").text(value.OtPay15));
                                                            row.append($("<td>").text(value.Gross));
                                                            row.append($("<td>").text(value.Epf8));
                                                            row.append($("<td>").text(value.Payee));
                                                            row.append($("<td>").text(value.Loan));
                                                            row.append($("<td>").text(value.OtherDeduction));
                                                            row.append($("<td>").text(value.CashShort));
                                                            row.append($("<td>").text(value.TotalDeduction));
                                                            row.append($("<td>").text(value.NetSalary));
                                                            row.append($("<td>").text(value.EPF12));
                                                            row.append($("<td>").text(value.ETF3));
                                                            row.append($("<td>").text(value.EPF20));




                                                            table.append(row);
                                                        });
                                                    },
                                                    complete: function () {
                                                        $("#loading").css("visibility", "hidden");
                                                    }
                                                });
                                            }

                                        </script>
                                    </div>

                                </div>
                                <div class="col-md-2">
                                    <div class="form-group has-success">

                                        <div class="col-md-10">
                                            <select class="form-control select" data-style="btn-success" onchange="loadSummary()" runat="server" id="list_month">
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
                                            <script type="text/javascript">
                                                function loadSummary() {

                                                    $.ajax({
                                                        beforeSend: function () {
                                                            $("#loading").css("visibility", "visible");
                                                        },
                                                        url: "https://localhost:44341/api/Payroll/GetSummaryMonth?year=" + document.getElementById('list_date').value + "&month=" + document.getElementById('list_month').value + "&searchmode=" + document.getElementById('list_mode').value,
                                                        success: function (data) {
                                                            var table = $("#customers2 tbody");
                                                            table.empty();
                                                            $.each(data, function (index, value) {
                                                                var row = $("<tr>");
                                                                row.append($("<td>").text(value.Index));
                                                                row.append($("<td>").text(value.EpfNo));
                                                                row.append($("<td>").text(value.Name));
                                                                row.append($("<td>").text(value.Line));
                                                                row.append($("<td>").text(value.Basic));
                                                                row.append($("<td>").text(value.Budgatry));
                                                                row.append($("<td>").text(value.PHAllowance));
                                                                row.append($("<td>").text(value.Late));
                                                                row.append($("<td>").text(value.NoPay));
                                                                row.append($("<td>").text(value.TotalForEPF));
                                                                row.append($("<td>").text(value.TotalOtherAllowance));
                                                                row.append($("<td>").text(value.OtHours15));
                                                                row.append($("<td>").text(value.OtPay15));
                                                                row.append($("<td>").text(value.Gross));
                                                                row.append($("<td>").text(value.Epf8));
                                                                row.append($("<td>").text(value.Payee));
                                                                row.append($("<td>").text(value.Loan));
                                                                row.append($("<td>").text(value.OtherDeduction));
                                                                row.append($("<td>").text(value.CashShort));
                                                                row.append($("<td>").text(value.TotalDeduction));
                                                                row.append($("<td>").text(value.NetSalary));
                                                                row.append($("<td>").text(value.Epf12));
                                                                row.append($("<td>").text(value.Etf3));
                                                                row.append($("<td>").text(value.Epf20));




                                                                table.append(row);
                                                            });
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
                                <div class="col-md-2">
                                    <div class="form-group has-success">
                                        <div class="col-md-10">
                                            <asp:DropDownList class="form-control select" data-style="btn-success" onchange="abc();" runat="server" ID="list_paymode">
                                                <asp:ListItem>All Employees Bank | Cash Pay</asp:ListItem>
                                                <asp:ListItem>Bank Pay Employees Only</asp:ListItem>
                                                <asp:ListItem>Cash Pay Employees Only</asp:ListItem>
                                            </asp:DropDownList>

                                            <script type="text/javascript">
                                                function abc() {
                                                    $.ajax({
                                                        beforeSend: function () {
                                                            $("#loading").css("visibility", "visible");
                                                        },
                                                        url: "https://localhost:44341/api/Payroll/GetSummaryMonth?year=" + document.getElementById('list_date').value + "&month=" + document.getElementById('list_month').value + "&searchmode=" + document.getElementById('list_paymode').value + "&searchmode2=" + document.getElementById('list_epf').value,
                                                        success: function (data) {
                                                            var table = $("#customers2 tbody");
                                                            table.empty();
                                                            $.each(data, function (index, value) {
                                                                var row = $("<tr>");
                                                                row.append($("<td>").text(value.Index));
                                                                row.append($("<td>").text(value.EpfNo));
                                                                row.append($("<td>").text(value.Name));
                                                                row.append($("<td>").text(value.Line));
                                                                row.append($("<td>").text(value.Basic));
                                                                row.append($("<td>").text(value.Budgatry));
                                                                row.append($("<td>").text(value.PHAllowance));
                                                                row.append($("<td>").text(value.Late));
                                                                row.append($("<td>").text(value.NoPay));
                                                                row.append($("<td>").text(value.TotalForEPF));
                                                                row.append($("<td>").text(value.TotalOtherAllowance));
                                                                row.append($("<td>").text(value.OtHours15));
                                                                row.append($("<td>").text(value.OtPay15));
                                                                row.append($("<td>").text(value.Gross));
                                                                row.append($("<td>").text(value.Epf8));
                                                                row.append($("<td>").text(value.Payee));
                                                                row.append($("<td>").text(value.Loan));
                                                                row.append($("<td>").text(value.OtherDeduction));
                                                                row.append($("<td>").text(value.CashShort));
                                                                row.append($("<td>").text(value.TotalDeduction));
                                                                row.append($("<td>").text(value.NetSalary));
                                                                row.append($("<td>").text(value.Epf12));
                                                                row.append($("<td>").text(value.Etf3));
                                                                row.append($("<td>").text(value.Epf20));




                                                                table.append(row);
                                                            });
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
                                <div class="col-md-2">
                                    <div class="form-group has-success">
                                        <div class="col-md-10">
                                            <asp:DropDownList class="form-control select" data-style="btn-success" onchange="abc2();" runat="server" ID="list_epf" BackColor="#999966">
                                                <asp:ListItem>All Employees (EPF Pay | Not Pay)</asp:ListItem>
                                                <asp:ListItem>EPF Pay Employees Only</asp:ListItem>
                                                <asp:ListItem>EPF Not Pay Employees Only</asp:ListItem>
                                            </asp:DropDownList>
                                            <script type="text/javascript">
                                                function abc2() {
                                                    $.ajax({
                                                        beforeSend: function () {
                                                            $("#loading").css("visibility", "visible");
                                                        },
                                                        url: "https://localhost:44341/api/Payroll/GetSummaryMonth?year=" + document.getElementById('list_date').value + "&month=" + document.getElementById('list_month').value + "&searchmode=" + document.getElementById('list_paymode').value + "&searchmode2=" + document.getElementById('list_epf').value,
                                                        success: function (data) {
                                                            var table = $("#customers2 tbody");
                                                            table.empty();
                                                            $.each(data, function (index, value) {
                                                                var row = $("<tr>");
                                                                row.append($("<td>").text(value.Index));
                                                                row.append($("<td>").text(value.EpfNo));
                                                                row.append($("<td>").text(value.Name));
                                                                row.append($("<td>").text(value.Line));
                                                                row.append($("<td>").text(value.Basic));
                                                                row.append($("<td>").text(value.Budgatry));
                                                                row.append($("<td>").text(value.PHAllowance));
                                                                row.append($("<td>").text(value.Late));
                                                                row.append($("<td>").text(value.NoPay));
                                                                row.append($("<td>").text(value.TotalForEPF));
                                                                row.append($("<td>").text(value.TotalOtherAllowance));
                                                                row.append($("<td>").text(value.OtHours15));
                                                                row.append($("<td>").text(value.OtPay15));
                                                                row.append($("<td>").text(value.Gross));
                                                                row.append($("<td>").text(value.Epf8));
                                                                row.append($("<td>").text(value.Payee));
                                                                row.append($("<td>").text(value.Loan));
                                                                row.append($("<td>").text(value.OtherDeduction));
                                                                row.append($("<td>").text(value.CashShort));
                                                                row.append($("<td>").text(value.TotalDeduction));
                                                                row.append($("<td>").text(value.NetSalary));
                                                                row.append($("<td>").text(value.Epf12));
                                                                row.append($("<td>").text(value.Etf3));
                                                                row.append($("<td>").text(value.Epf20));




                                                                table.append(row);
                                                            });
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
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <asp:Button ID="btn_load" runat="server" Text="Load" CssClass="form-control" BackColor="#009933" ForeColor="White" Font-Bold="true" OnClientClick="loadSummarybtn()"></asp:Button>
                                        <script type="text/javascript">
                                            const button2 = document.querySelector("#btn_load");

                                            button2.addEventListener("click", function () {
                                                myMethod2();
                                                event.preventDefault();
                                            });
                                            function myMethod2() {

                                                $.ajax({
                                                    beforeSend: function () {
                                                        $("#loading").css("visibility", "visible");
                                                    },
                                                    url: "https://localhost:44341/api/Payroll/GetSummaryMonth?year=" + document.getElementById('list_date').value + "&month=" + document.getElementById('list_month').value + "&searchmode=" + document.getElementById('list_paymode').value + "&searchmode2=" + document.getElementById('list_epf').value,
                                                    success: function (data) {
                                                        var table = $("#customers2 tbody");
                                                        table.empty();
                                                        $.each(data, function (index, value) {
                                                            var row = $("<tr>");
                                                            row.append($("<td>").text(value.Index));
                                                            row.append($("<td>").text(value.EpfNo));
                                                            row.append($("<td>").text(value.Name));
                                                            row.append($("<td>").text(value.Line));
                                                            row.append($("<td>").text(value.Basic));
                                                            row.append($("<td>").text(value.Budgatry));
                                                            row.append($("<td>").text(value.PHAllowance));
                                                            row.append($("<td>").text(value.Late));
                                                            row.append($("<td>").text(value.NoPay));
                                                            row.append($("<td>").text(value.TotalForEPF));
                                                            row.append($("<td>").text(value.TotalOtherAllowance));
                                                            row.append($("<td>").text(value.OtHours15));
                                                            row.append($("<td>").text(value.OtPay15));
                                                            row.append($("<td>").text(value.Gross));
                                                            row.append($("<td>").text(value.Epf8));
                                                            row.append($("<td>").text(value.Payee));
                                                            row.append($("<td>").text(value.Loan));
                                                            row.append($("<td>").text(value.OtherDeduction));
                                                            row.append($("<td>").text(value.CashShort));
                                                            row.append($("<td>").text(value.TotalDeduction));
                                                            row.append($("<td>").text(value.NetSalary));
                                                            row.append($("<td>").text(value.Epf12));
                                                            row.append($("<td>").text(value.Etf3));
                                                            row.append($("<td>").text(value.Epf20));
                                                            table.append(row);
                                                        });
                                                    },
                                                    complete: function () {
                                                        $("#loading").css("visibility", "hidden");
                                                    }
                                                });
                                            }

                                        </script>
                                    </div>

                                </div>
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <asp:Button ID="btn_export" runat="server" Text="Export PDF" CssClass="form-control" BackColor="#009933" ForeColor="White" Font-Bold="true" OnClick="btn_export_Click"></asp:Button>

                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-12">

                            <!-- START DATATABLE EXPORT -->
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Salary Summary</h3>
                                    <div class="btn-group pull-right">
                                        <button class="btn btn-danger dropdown-toggle" data-toggle="dropdown"><i class="fa fa-bars"></i>Export Data</button>
                                        <ul class="dropdown-menu">

                                            <li><a href="#" onclick="$('#customers2').tableExport({type:'excel',escape:'false'});">
                                                <img src='img/icons/xls.png' width="24" />
                                                XLS</a></li>

                                        </ul>
                                    </div>

                                </div>
                                <div class="panel-body">
                                    <div style="width: 100%; overflow-x: auto; height: 450px;">
                                        <table id="customers2" class="table active" style="overflow-x: auto;">
                                            <thead>
                                                <tr>
                                                    <th>No</th>
                                                    <th>EPF No</th>
                                                    <th>Employee</th>
                                                    <th>Department</th>
                                                    <th>Basic</th>
                                                    <th>Budj.</th>
                                                    <th>PHAllowance</th>
                                                    <th>Pay Cut</th>
                                                    <th>NoPay</th>
                                                    <th>Total For EPF</th>
                                                    <th>Other Allowance</th>
                                                    <th>OT Hours 1.5</th>
                                                    <th>OT Pay 1.5</th>
                                                    <th>Gross Salary</th>
                                                    <th>EPF 8%</th>
                                                    <th>Payee</th>
                                                    <th>Loan</th>
                                                    <th>Other Deduction</th>
                                                    <th>Cash Short</th>
                                                    <th>Total Deduction</th>
                                                    <th>Net Salary</th>
                                                    <th>EPF 12%</th>
                                                    <th>ETF 3%</th>
                                                    <th>ETF 20%</th>
                                                </tr>
                                            </thead>
                                            <tbody></tbody>
                                        </table>
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
        <div id="loading" runat="server">
            <img src="images/Loading_2.gif" alt="Loading...">
        </div>
        <!-- END MESSAGE BOX-->

        <!-- START PRELOADS -->
        <audio id="audio-alert" src="audio/alert.mp3" preload="auto"></audio>
        <audio id="audio-fail" src="audio/fail.mp3" preload="auto"></audio>
        <!-- END PRELOADS -->

        <!-- START SCRIPTS -->
        <!-- START PLUGINS -->
        <script type="text/javascript" src="js/plugins/jquery/jquery.min.js"></script>
        <script type="text/javascript" src="js/plugins/jquery/jquery-ui.min.js"></script>
        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap.min.js"></script>
        <!-- END PLUGINS -->

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

        <script type='text/javascript' src='js/plugins/icheck/icheck.min.js'></script>
        <script type="text/javascript" src="js/plugins/mcustomscrollbar/jquery.mCustomScrollbar.min.js"></script>

        <script type="text/javascript" src="js/plugins/datatables/jquery.dataTables.min.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/tableExport.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/jquery.base64.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/html2canvas.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/jspdf/libs/sprintf.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/jspdf/jspdf.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/jspdf/libs/base64.js"></script>
        <!-- END THIS PAGE PLUGINS-->
        <!-- START TEMPLATE -->
        <script type="text/javascript" src="js/settings.js"></script>

        <script type="text/javascript" src="js/plugins.js"></script>
        <script type="text/javascript" src="js/actions.js"></script>
    </form>
</body>
</html>
