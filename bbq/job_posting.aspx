<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="job_posting.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="bbq.job_posting" EnableEventValidation="false" %>

<!DOCTYPE html>
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
    <script>
        function openNewTab() {
            window.open('http://geryjdakdai-001-site8.atempurl.com/', '_blank');
            return false; // To prevent the postback of the button
        }
    </script>

    <script type="text/javascript">
        function loadJobList() {

            $.ajax({
                url: "https://localhost:44341/api/Job/getJobList",

                success: function (data) {
                    var table2 = $("#table tbody");
                    table2.empty();
                    function createDeleteButton() {
                        var deleteButton = $("<button>").text("Un-List").attr("class", "btn btn-danger btn-rounded btn-sm");
                        deleteButton.click(function () {

                            var value = $(this).closest("tr").find("td:first").text();
                            if (confirm("Are you sure you want to delete Selected Job Lisitng ?")) {
                                $(this).closest("tr").remove();
                                $.ajax({
                                    beforeSend: function () {
                                        $("#loading").css("visibility", "visible");
                                    },
                                    url: "https://localhost:44341/api/Job/DeleteJob?id=" + value,
                                    type: "DELETE",
                                    success: function (data) {
                                        alert("Selected Lisiting Deleted Successfully");

                                    },
                                    complete: function () {
                                        $("#loading").css("visibility", "hidden");
                                    }
                                });
                            }
                        });
                        return deleteButton;
                    }
                    var headerRow = $("<tr>");
                    headerRow.append($("<th>").text("Job Title"));
                    headerRow.append($("<th>").text("Status"));
                    headerRow.append($("<th>").text("Active | Hold"));
                    headerRow.append($("<th>").text("Actions"));
                    table2.append(headerRow);
                    $.each(data, function (index, value) {
                        var row = $("<tr>");
                        row.append($("<td>").css("display", "none").text(value.Id));
                        row.append($("<td>").text(value.JobTitle));
                        row.append($("<td>").text(value.Status));
                        checkbox = $("<input>").attr(
                            {
                                type: "checkbox",
                                checked: value.StatusValue
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

                            chnageCheckBox(row.find('td:eq(0)').text(), checkboxState);

                        });
                        row.append($("<td>").append(createDeleteButton()));
                        table2.append(row);
                    });

                },
                complete: function () {
                    $("#loading").css("visibility", "hidden");
                }
            });
        }
        window.onload = function () {
            loadJobList();
        };
    </script>
    <script type="text/javascript">
        function chnageCheckBox(id, status) {
            $.ajax({
                beforeSend: function () {
                    $("#loading").css("visibility", "visible");
                },
                url: "https://localhost:44341/api/Job/UpdateJob?id=" + id + "&status=" + status,
                type: "PUT",
                success: function (data) {

                    alert("Successfully Updated Selected Joblist");
                    loadJobList();
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
                    $("#loading").css("visibility", "hidden");
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
                    <li class="xn-openable">
                        <a href="#"><span class="fa fa-pencil"></span><span class="xn-text">Reports</span></a>
                        <ul>
                            <li><a href="report_salary_summary.aspx"><span class="fa fa-file-text-o"></span>Salary Summary (Employee)</a></li>
                            <li><a href="advance_reports.aspx"><span class="fa fa-file-text-o"></span>Other Reports</a></li>
                        </ul>
                    </li>
                    <li class="xn-openable active">
                        <a href="#"><span class="fa fa-bitbucket"></span><span class="xn-text">Recruitment</span></a>
                        <ul>
                            <li class="active"><a href="job_posting.aspx"><span class="fa fa-heart"></span>Job Posting</a></li>
                            <li><a href="applicant_dashboard.aspx"><span class="fa fa-square-o"></span>Applicant DashBoard</a></li>
                            <li><a href="interview.aspx"><span class="fa fa-square-o"></span>Interview</a></li>
                            <li><a href="onboarding.aspx"><span class="fa fa-square-o"></span>Onboarding</a></li>
                        </ul>
                    </li>
                    <li class="xn-openable">
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
                    <li class="xn-icon-button pull-right"></li>
                    <!-- END MESSAGES -->
                    <!-- TASKS -->
                    <li class="xn-icon-button pull-right"></li>
                    <!-- END TASKS -->
                </ul>
                <!-- END X-NAVIGATION VERTICAL -->

                <!-- START BREADCRUMB -->
                <ul class="breadcrumb">
                    <li><a href="#">Home</a></li>
                    <li class="active">Job Posting</li>
                </ul>
                <!-- END BREADCRUMB -->

                <!-- PAGE CONTENT WRAPPER -->
                <div class="page-content-wrap">

                    <div class="row">
                        <div class="col-md-14">

                            <div class="block">

                                <div class="col-md-6">
                                    <div class="panel panel-default">

                                        <div class="panel-heading">
                                            <h3 class="panel-title">Create New Job List</h3>
                                        </div>

                                        <div class="panel-body ">

                                            <div class="form-group has-success">
                                                <label class="control-label">Job Title</label>
                                                <asp:TextBox ID="text_jobtitle" runat="server" CssClass="form-control" Placeholder="job title"> </asp:TextBox>
                                            </div>
                                            <div class="form-group has-success">
                                                <label class="control-label">Logo ( Job Lisitng ) </label>
                                                <asp:FileUpload CssClass="form-control" ID="FileUpload" runat="server" BorderColor="#3399ff" accept=".jpeg,.jpg,.png" EnableViewState="false"></asp:FileUpload>
                                            </div>
                                            <div class="form-group has-success">
                                                <label class="control-label">Job Category</label>
                                                <asp:DropDownList class="form-control" ID="list_jobcategory" runat="server" Font-Size="Small" AutoPostBack="true" OnSelectedIndexChanged="list_jobcategory_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group has-success">
                                                <label class="control-label">Job Type</label>
                                                <asp:DropDownList class="form-control" ID="list_jobtype_" runat="server" Font-Size="Small" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group has-success">
                                                <label class="col-md-2 control-label">Full Time</label>
                                                <div class="col-md-4">
                                                    <label class="switch">
                                                        <input type="checkbox" checked value="0" runat="server" id="check_fulltime" />
                                                        <span></span>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group has-success">
                                                <label class="col-md-2 control-label">Part Time</label>
                                                <div class="col-md-4">
                                                    <label class="switch">
                                                        <input type="checkbox" checked value="1" runat="server" id="check_parttime" />
                                                        <span></span>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group has-success">
                                                <label class="control-label">Job Description</label>

                                                <textarea class="form-control" runat="server" id="text_description"></textarea>

                                            </div>
                                            <div class="form-group has-success">
                                                <label class="control-label">Job Type</label>
                                                <div class="form-group has-success">
                                                    <select class="form-control select" runat="server" id="list_jobtype">
                                                        <option>Permant</option>
                                                        <option>Contract</option>
                                                        <option>Internship</option>
                                                        <option>Temporary</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="form-group has-success">
                                                <label class="control-label">Required Qulification</label>

                                                <textarea class="form-control" runat="server" id="text_qulification"></textarea>

                                            </div>
                                            <div class="form-group has-success">
                                                <label class="control-label">We offer</label>

                                                <textarea class="form-control" runat="server" id="text_weoffer"></textarea>

                                            </div>
                                            <div class="form-group has-success">

                                                <div class="col-md-4">
                                                    <label class="control-label">Application Deadline</label>
                                                    <asp:TextBox ID="date_applicationdeadline" CssClass="form-control" runat="server" TextMode="Date"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group has-success">

                                                <div class="col-md-4">
                                                    <label class="control-label">Contact Email</label>
                                                    <asp:TextBox ID="text_email" CssClass="form-control" runat="server" Placeholder="email"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group has-success">

                                                <div class="col-md-4">
                                                    <label class="control-label">Contact Number</label>
                                                    <asp:TextBox ID="text_contact" CssClass="form-control" runat="server" Placeholder="contact"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="panel-footer">
                                            <asp:Button ID="btn_save" class="btn btn-primary pull-right" Text="Publish Job" runat="server" OnClick="btn_save_Click1" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="panel panel-default">

                                        <div class="panel-heading">

                                            <div>
                                                <asp:Button ID="btn_JobOpen" class="btn btn-primary pull-right" Text="Open Job DashBoard" runat="server" OnClientClick="openNewTab();" BackColor="#ff9933" BorderColor="#ff9933" />
                                            </div>
                                            <h3 class="panel-title">Listed Job's</h3>
                                        </div>

                                        <div class="panel-body ">

                                            <div class="panel-body panel-body-table">

                                                <div class="table-responsive">
                                                    <table class="table table-bordered table-striped table-actions" id="table" runat="server">
                                                        <thead>
                                                            <tr>
                                                                <th width="100">Id</th>
                                                                <th width="100">Title</th>
                                                                <th width="100">Status</th>
                                                                <th width="100">Hold/Un-Hold</th>
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
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>


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

</body>

