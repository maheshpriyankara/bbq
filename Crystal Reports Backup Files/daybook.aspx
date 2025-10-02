<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="daybook.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="bbq.daybook" EnableEventValidation="false" %>

<!DOCTYPE html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>ERP</title>
    <link href="jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="jquery.min.js" type="text/javascript"></script>
    <script src="jquery-ui.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            SearchText();
        });
        function SearchText() {

            $("#category").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "double_entry.aspx/getAccount",
                        data: "{'empName':'" + document.getElementById('category').value + "','index_':'" + document.getElementById('category_type').selectedIndex + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            alert("No Match");
                        }
                    });
                }
            });
        }
    </script>
    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link href="https://fonts.googleapis.com/css2?family=Nunito:wght@300;400;600;700;800&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="../assets/css/bootstrap.css">

    <link rel="stylesheet" href="../assets/vendors/iconly/bold.css">

    <link rel="stylesheet" href="../assets/vendors/perfect-scrollbar/perfect-scrollbar.css">
    <link rel="stylesheet" href="../assets/vendors/bootstrap-icons/bootstrap-icons.css">
    <link rel="stylesheet" href="../assets/css/app.css">
    <link rel="shortcut icon" href="../assets/images/favicon.svg" type="image/x-icon">


    <script type="text/javascript">



        function yesnoCheck() {


            if (document.getElementById('radio_cheque').checked) {
                document.getElementById('bt_cheque_refrence').style.visibility = 'visible';
            } else {
                document.getElementById('bt_cheque_refrence').style.visibility = 'hidden';

            }
        }

    </script>



</head>

<body>
    <form id="Form1" runat="server">
        <div id="app">
            <div id="sidebar" class="active">
                <div class="sidebar-wrapper active">
                    <div class="sidebar-header">
                        <div class="d-flex justify-content-between">
                            <div class="logo">
                                <a href="index.html">
                                    <img src="../assets/images/logo/logo.png" alt="Logo" srcset=""></a>
                            </div>
                            <div class="toggler">
                                <a href="#" class="sidebar-hide d-xl-none d-block"><i class="bi bi-x bi-middle"></i></a>
                            </div>
                        </div>
                    </div>
                    <div class="sidebar-menu">
                        <ul class="menu">
                            <li class="sidebar-title">Home</li>


                            <li class="sidebar-item has-sub" id="li_dashboard" runat="server">
                                <a href="#" class='sidebar-link'>
                                    <i class="bi bi-stack"></i>
                                    <span>Dashboard</span>
                                </a>
                                <ul class="submenu">
                                    <li class="submenu-item ">
                                        <a href="dashboard.aspx">Finance</a>
                                    </li>
                                     <li class="submenu-item">
                                        <a href="customer.aspx">Customer</a>
                                    </li>
                                </ul>
                            </li>




                            <li class="sidebar-item active has-sub" id="menu_data_entry" runat="server">
                                <a href="#" class='sidebar-link'>
                                    <i class="bi bi-hexagon-fill"></i>
                                    <span>Summary</span>
                                </a>
                                <ul class="submenu active">
                                    <li class="submenu-item active">
                                        <a href="daybook.aspx">Daybook</a>
                                    </li>

                                </ul>
                            </li>









                            <li class="sidebar-title"><a href="default.aspx">Sign Out</a></li>
                        </ul>
                    </div>
                    <button class="sidebar-toggler btn x"><i data-feather="x"></i></button>
                </div>
            </div>
            <div id="main">
                <header class="mb-3">
                    <a href="#" class="burger-btn d-block d-xl-none">
                        <i class="bi bi-justify fs-3"></i>
                    </a>
                </header>


                <div class="page-content">

                    <section class="row">
                        <div class="col-12 col-lg-9">
                            <div class="row">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:TextBox CssClass="form-control" ID="text_date_from" runat="server" BorderColor="#3399ff" TextMode="Date"></asp:TextBox>
                                                </div>

                                            </div>
                                             <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="form-group">
                                                        <asp:DropDownList CssClass="form-control" ID="branch" runat="server" OnSelectedIndexChanged="account_type_SelectedIndexChanged" AutoPostBack="True" BorderColor="#3399ff">
                                                            <asp:ListItem>Hardware</asp:ListItem>
                                                            <asp:ListItem>Moratuwa</asp:ListItem>
                                                            <asp:ListItem>Panadura</asp:ListItem>
                                                            <asp:ListItem>Rawatawatta</asp:ListItem>
                                                            <asp:ListItem>Pamankada</asp:ListItem>
                                                        </asp:DropDownList>
                                                        
                                                    </div>
                                                </div>
                                            </div>
                                             <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Button ID="Button2" CssClass="btn btn-primary pull-right" runat="server" Text="Load" OnClick="Button2_Click" AutoPostBack="False" />

                                                </div>

                                            </div>
                                        </div>


                                       



                                    </div>

                                </div>
                            </div>
                        </div>

                    </section>
                    <section class="row">
                        <div class="col-12 ">
                            <div class="row">
                                <div class="card">

                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="bmd-label-floating">Total Deposit</label>
                                                    <asp:TextBox CssClass="form-control" ID="text_deposit" runat="server" BorderColor="#3399ff" Enabled="false"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="bmd-label-floating">Total BL</label>
                                                    <asp:TextBox CssClass="form-control" ID="text_bl" runat="server" BorderColor="#3399ff" Enabled="false"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="bmd-label-floating">Total EX</label>
                                                    <asp:TextBox CssClass="form-control" ID="text_ex" runat="server" BorderColor="#3399ff" Enabled="false"></asp:TextBox>
                                                </div>

                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="bmd-label-floating">Direct Sale</label>
                                                    <asp:TextBox CssClass="form-control" ID="text_sale" runat="server" BorderColor="#3399ff" Enabled="false"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="bmd-label-floating">Cash Balance</label>
                                                    <asp:TextBox CssClass="form-control" ID="text_cash" runat="server" BorderColor="#3399ff" Enabled="false"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="bmd-label-floating">Total Discount</label>
                                                    <asp:TextBox CssClass="form-control" ID="text_discount" runat="server" BorderColor="#3399ff" Enabled="false"></asp:TextBox>
                                                </div>

                                            </div>

                                        </div>
                                        <asp:GridView ID="table1" runat="server" AutoGenerateColumns="true" AutoPostback="Flash"
                                            CssClass="table table-striped" OnRowDeleting="GridView1_RowDeleting" OnRowCommand="table1_RowCommand">
                                        </asp:GridView>

                                    </div>
                                </div>
                            </div>
                        </div>

                    </section>
                </div>

                <footer>
                    <div class="footer clearfix mb-0 text-muted">
                        <div class="float-start">
                            <p>2022 &copy; Blind Cat Solution</p>
                        </div>

                    </div>
                </footer>
            </div>
        </div>

    </form>
    <script src="../assets/vendors/perfect-scrollbar/perfect-scrollbar.min.js"></script>
    <script src="../assets/js/bootstrap.bundle.min.js"></script>

    <script src="../assets/vendors/apexcharts/apexcharts.js"></script>
    <script src="../assets/js/pages/dashboard.js"></script>
    <script src="../assets/js/mazer.js"></script>


    <script src="../assets/vendors/perfect-scrollbar/perfect-scrollbar.min.js"></script>
    <script src="../assets/js/bootstrap.bundle.min.js"></script>

    <script src="../assets/vendors/simple-datatables/simple-datatables.js"></script>
    <script>
        // Simple Datatable
        var table1 = document.querySelector('#table1');
        var dataTable = new simpleDatatables.DataTable(table1);
    </script>





</body>

