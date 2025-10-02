<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="bbq._default" %>

<!DOCTYPE html>
<html lang="zxx">

<head>
    <title>ERP</title>
    <!-- Meta-Tags -->
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta charset="utf-8">
    <meta name="keywords" content="Ripley and BBK, Ripley & Marshall,BBK Partnership">
    <script>
        addEventListener("load", function () {
            setTimeout(hideURLbar, 0);
        }, false);

        function hideURLbar() {
            window.scrollTo(0, 1);
        }
    </script>
    <!-- //Meta-Tags -->
    <!-- Index-Page-CSS -->
    <link rel="stylesheet" href="css/style.css" type="text/css" media="all">
    <!-- //Custom-Stylesheet-Links -->
    <!--fonts -->
    <link href="//fonts.googleapis.com/css?family=Mukta+Mahee:200,300,400,500,600,700,800" rel="stylesheet">
    <!-- //fonts -->
    <!-- Font-Awesome-File -->
    <link rel="stylesheet" href="css/font-awesome.css" type="text/css" media="all">
</head>

<body>
    
    <div class="content-w3ls">
        <div class="agileits-grid">
            <div class="content-top-agile">
                <h2 style="text-align:center">LD Lucky</h2>
            </div>
            <div class="content-bottom">
                <form action="#" method="post" runat="server">
                    <div class="field_w3ls" runat="server">
                        <div class="field-group" runat="server">
                            <input runat="server" name="userID" id="text1" type="text" value="" placeholder="username" required>
                        </div>
                        <div class="field-group">
                            <input runat="server" id="password_field" type="password" class="form-control" name="password" value="" placeholder="Password">
                  
                            <span runat="server" toggle="#password-field" class="fa fa-fw fa-eye field-icon toggle-password"></span>
                        </div>
                    </div>
                    <div class="wthree-field">
                        <asp:Button ID="Button1" runat="server" Text="Login" OnClick="Button1_Click3" />
                    </div>
                   
                </form>
            </div>
            <!-- //content bottom -->
        </div>
    </div>
    <!--//copyright-->
    <script src="js/jquery-2.2.3.min.js"></script>
    <!-- script for show password -->
    <script>
        $(".toggle-password").click(function () {

            $(this).toggleClass("fa-eye fa-eye-slash");
            var input = $($(this).attr("toggle"));
            if (input.attr("type") == "password") {
                input.attr("type", "text");
            } else {
                input.attr("type", "password");
            }
        });
    </script>
    <!-- /script for show password -->

</body>
<!-- //Body -->

</html>
