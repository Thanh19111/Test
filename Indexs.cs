---ajax--
<script type="text/javascript">
    $(document).ready(function () {
        $("#btnSave").click(function () {
            if ($("#hoten").val() == "" || $("#tuoi").val() == "" || $("#diachi").val() == "" || $("#maphong").val() == "" || $("#luongnv").val() == "" || $("#matkhau").val() == "") {
                $("#msg").html("Dữ liệu không được để trống");
            } else {

                var emp = {};
                    emp.hotennv = $("#hotennv").val(),
                    emp.tuoi = $("#tuoi").val(),
                    emp.diachi = $("#diachi").val(),
                    emp.maphong = $("#maphong").val(),
                    emp.luongnv = $("#luongnv").val(),
                    emp.matkhau = $("#matkhau").val(),
                    $.ajax({
                        type: "POST",
                        url: '@Url.Action("/Create")',
                        data: '{emp:' + JSON.stringify(emp) + "}",
                        dataType: "json",
                        contentType: "application/json;charset=UTF-8",
                        success: function (res) {
                            if (res.a == true) {
                                $("#msg").html("Thêm thành công");
                            } else {
                                $("#msg").html(res.error);
                            }
                        },
                        error: function () {
                            alert("Có lỗi xảy ra")
                        }

                    });
                return false;
            }
        });
    });
</script>
---2----
<script type="text/javascript">
$(function(){
    $("#btnSave").click(function()
    {
        var nv = {};
        nv.hotennv = $("#hotennv").val();
        nv.tuoi = $("#tuoi").val();
        nv.diachi = $("#diachi").val();
        nv.luongnv = $("#luongnv").val();
        nv.maphong = $("#maphong").val();
        nv.matkhau = $("#matkhau").val();

        $.ajax({
            type: "POST",
            url: "@Url.Action("/Create")",
            data: '{nv:' + JSON.stringify(nv) + '}',
            dataType: 'Json',
            contentType: 'application/json; charset = UTF-8',
            success: function(res)
            {
                if(res.result == true)
                {
                    $('#mess').html('Thêm thành công');
                }else
                {
                    $('#mess').html(res.error);
                }
            },
            error: function(err)
            {
                alert("Lỗi khi thêm" + err);
            }
        });
        return false;
    });
});
</script>
--------------
---them--
[HttpPost]
public ActionResult Create(Nhanvien emp)
{
    try
    {
        db.Nhanviens.Add(emp);
        db.SaveChanges();
        return Json(new { a = true, JsonRequestBehavior.AllowGet });
    }catch(Exception ex)
    {
        return Json(new { a = false, error = ex.Message});
    }
}
--login -logout
[HttpGet]
public ActionResult Login()
{
    return View();
}

[HttpPost]
public ActionResult Login (String manv, String pass)
{
    try
    {
        int ma = int.Parse(manv);
        var nv = db.Nhanviens.FirstOrDefault(x => x.manv == ma);
        if(nv!= null && nv.matkhau.Equals(pass))
        {
            Session["user"] = nv.hotennv;
            return RedirectToAction("Index");
        }else
        {
            ViewBag.Message = "Tài khoản hoặc mật khẩu không chính xác";
            return View("Login");
        }
    }catch
    {
        ViewBag.Message = "Dữ liệu nhập vào không hợp lệ";
        return View("Login");
    }
    
}
public ActionResult Logout()
{
    Session.Remove("user");
    return RedirectToAction("Index");
}
--Menu--
routes.MapMvcAttributeRoutes();

public ActionResult Menu()
{
    return PartialView(db.Phongbans.ToList());
}

[Route("phongban/{ma?}")]
public ActionResult ByID(int ma)
{
    return View(db.Nhanviens.Where(x=>x.maphong == ma).ToList());
}

--------------------------------------------------------------------
<style>
    .menu{
        background-color: black;
        height: 50px;
    }
    .menu ul {
        display: flex;
        justify-content: center;
        align-items: flex-end;
        list-style-type: none;
    }
    .menu li {
        width: 150px;
        color: white;
        line-height: 50px;
    }
    .menu a{
        text-decoration: none;
        color: white;
        line-height:50px;
    }

</style>
----------------------------
@model IEnumerable<WebApplication1.Models.Phongban>
    @foreach (var item in Model)
    {
        <li>
            <a href="/phongban/@item.maphong">@item.tenphong</a>
        </li>
    }
----------
<div class="menu">
    <ul>
        <li style="color: white">@Html.ActionLink("Thêm Nhân Viên", "Create", "Nhanvien", new { area = "" }, new { @class = "nav-link" })</li>
        <li style="color: white">@Html.ActionLink("Xem danh sach", "Xemdanhsach", "Nhanvien", new { area = "" }, new { @class = "nav-link" })</li>
        @{
            Html.RenderAction("Menu", "Nhanvien");
        }
        @if (Session["user"] != null)
        {
            <li style="color: white">@Html.ActionLink("Log out", "Logout", "Nhanvien", new { area = "" }, new { @class = "nav-link" })</li>
            <li style="width: 200px">
                Chào mừng bạn: @Session["user"]
            </li>
        }
        else
        {
            <li style="color: white">@Html.ActionLink("Log in", "Login", "Nhanvien", new { area = "" }, new { @class = "nav-link" })</li>
        }
    </ul>
</div>