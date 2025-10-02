<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sale_history.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="bbq.sale_history" EnableEventValidation="false" %>

<!DOCTYPE html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>ERP</title>
    <link href="jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="jquery.min.js" type="text/javascript"></script>
    <script src="jquery-ui.min.js" type="text/javascript"></script>


    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link href="https://fonts.googleapis.com/css2?family=Nunito:wght@300;400;600;700;800&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="../assets/css/bootstrap.css">

    <link rel="stylesheet" href="../assets/vendors/iconly/bold.css">

    <link rel="stylesheet" href="../assets/vendors/perfect-scrollbar/perfect-scrollbar.css">
    <link rel="stylesheet" href="../assets/vendors/bootstrap-icons/bootstrap-icons.css">
    <link rel="stylesheet" href="../assets/css/app.css">
    <link rel="shortcut icon" href="../assets/images/favicon.svg" type="image/x-icon">
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
                                    <li class="submenu-item ">
                                        <a href="daybook.aspx">Daybook</a>
                                    </li>

                                    <li class="submenu-item">
                                        <a href="customer.aspx">Customer</a>
                                    </li>
                                    <li class="submenu-item  active">
                                        <a href="sale_history.aspx">Outstanding ( Rent )</a>
                                    </li>
                                    <li class="submenu-item">
                                        <a href="stock.aspx">Stock</a>
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
                                            
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="form-group">
                                                        <asp:DropDownList CssClass="form-control" ID="combo_period" runat="server" OnSelectedIndexChanged="account_type_SelectedIndexChanged" AutoPostBack="True" BorderColor="#3399ff">
                                                            <asp:ListItem>Pending - Below 14 Days</asp:ListItem>
                                                            <asp:ListItem>Pending - More Than 14 Days</asp:ListItem>
                                                           
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="form-group">
                                                        <asp:DropDownList CssClass="form-control" ID="combo_location" runat="server" OnSelectedIndexChanged="account_type_SelectedIndexChanged" AutoPostBack="True" BorderColor="#3399ff">
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

