<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapPath.aspx.cs" Inherits="Web.MapPath" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <title>地图</title>
    <style type="text/css">
        body, html, #allmap { width: 100%; height: 100%; overflow: hidden; margin: 0; font-family: "微软雅黑"; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="allmap"></div>
    </form>
</body>
<script src="Js/jquery-1.11.0.min.js"></script>
<script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=6yDn0Bt6NUSnGiu7qTv4qtKGGo5jfXwz"></script>
<script type="text/javascript">
    // 百度地图API功能
    //var longitude = "113.99389";
    var longitude = $("#Tex_lgt").val();
    var latitude = $("#Tex_ltd").val();
    //var latitude = "22.68806";
    // 百度地图API功能
    var map = new BMap.Map("allmap");
    //map.centerAndZoom(new BMap.Point(longitude, latitude), 14);
    var point = new BMap.Point(longitude, latitude);
    map.centerAndZoom(point, 15);
    var marker = new BMap.Marker(point);  // 创建标注
    map.addOverlay(marker);               // 将标注添加到地图中
    marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
    map.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放
    //map.centerAndZoom("深圳", 15);      // 初始化地图,用城市名设置地图中心点
    map.addOverlay(marker);              // 将标注添加到地图中
    var label = new BMap.Label("目的位置", { offset: new BMap.Size(20, -10) });
    marker.setLabel(label);

    // 添加带有定位的导航控件
    var navigationControl = new BMap.NavigationControl({
        // 靠左上角位置
        anchor: BMAP_ANCHOR_TOP_LEFT,
        // LARGE类型
        type: BMAP_NAVIGATION_CONTROL_LARGE,
        // 启用显示定位
        enableGeolocation: true
    });
    map.addControl(navigationControl);
    // 添加定位控件
    var geolocationControl = new BMap.GeolocationControl();
    geolocationControl.addEventListener("locationSuccess", function (e) {
        // 定位成功事件
        var address = '';
        address += e.addressComponent.province;//定位的所在省份
        address += e.addressComponent.city; //定位所在市
        address += e.addressComponent.district; //定位所在区
        address += e.addressComponent.street;  //定位所在街
        address += e.addressComponent.streetNumber;   //定位所在街编号
        alert("当前定位地址为：" + address);
    });
    geolocationControl.addEventListener("locationError", function (e) {
        // 定位失败事件
        alert(e.message);
    });
    map.addControl(geolocationControl);
    //$(function () {
    //    // 百度地图API功能
    //    var longitude = "113.99389";
    //    var latitude = "22.68806";

    //    var map = new BMap.Map("#allmap");
    //    //map.centerAndZoom(new BMap.Point(longitude, latitude), 14);
    //    var point = new BMap.Point(113.99389, 22.68806);
    //    map.centerAndZoom(point, 15);

    //    var marker = new BMap.Marker(point);  // 创建标注
    //    map.addOverlay(marker);               // 将标注添加到地图中
    //    marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
    //    map.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放
    //    map.addOverlay(marker);              // 将标注添加到地图中
    //    var label = new BMap.Label("目的位置", { offset: new BMap.Size(20, -10) });
    //    marker.setLabel(label);
    //    // 添加带有定位的导航控件
    //    var navigationControl = new BMap.NavigationControl({
    //        // 靠左上角位置
    //        anchor: BMAP_ANCHOR_TOP_LEFT,
    //        // LARGE类型
    //        type: BMAP_NAVIGATION_CONTROL_LARGE,
    //        // 启用显示定位
    //        enableGeolocation: true
    //    });
    //    alert("11");
    //    map.addControl(navigationControl);
    //    // 添加定位控件
    //    var geolocationControl = new BMap.GeolocationControl();
    //    geolocationControl.addEventListener("locationSuccess", function (e) {
    //        // 定位成功事件
    //        var address = '';
    //        address += e.addressComponent.province;
    //        address += e.addressComponent.city;
    //        address += e.addressComponent.district;
    //        address += e.addressComponent.street;
    //        address += e.addressComponent.streetNumber;
    //        alert("当前定位地址为：" + address);
    //    });
    //    geolocationControl.addEventListener("locationError", function (e) {
    //        // 定位失败事件
    //        alert(e.message);
    //    });
    //    map.addControl(geolocationControl);
    //});
        
</script>
</html>
