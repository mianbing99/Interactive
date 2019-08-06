var MapApi = (function () {
    return {
        LoadLocationMap: function (lat, lng, containerId, showText) {
            ///<summary>载入地图</summary>  
            ///<param name="lat">纬度值</param>  
            ///<param name="lng">经度值</param>  
            ///<param name="containerId">地图容器ID,一般为Div的Id.</param>      
            var map = new BMap.Map(containerId);            // 创建Map实例  
            var point = new BMap.Point(lng, lat); // 创建点坐标  
            var marker = new BMap.Marker(point);  // 创建标注  
            map.addOverlay(marker);              // 将标注添加到地图中  
            map.centerAndZoom(point, 15);
            map.enableScrollWheelZoom();                 //启用滚轮放大缩小  
            var opts = {
                width: 50,     // 信息窗口宽度  
                height: 30,     // 信息窗口高度  
                title: showText, // 信息窗口标题  
                enableMessage: false,//设置允许信息窗发送短息  
                message: showText
            }
            var infoWindow = new BMap.InfoWindow("", opts);  // 创建信息窗口对象  
            map.openInfoWindow(infoWindow, point); //开启信息窗口      
        },
        LoadMap: function (lat, lng, panoramaContainerId, normalMapContainerId) {
            //全景图展示  
            var panorama = new BMap.Panorama(panoramaContainerId);
            panorama.setPosition(new BMap.Point(lng, lat)); //根据经纬度坐标展示全景图  
            panorama.setPov({ heading: -40, pitch: 6 });

            panorama.addEventListener('position_changed', function (e) { //全景图位置改变后，普通地图中心点也随之改变  
                var pos = panorama.getPosition();
                map.setCenter(new BMap.Point(pos.lng, pos.lat));
                marker.setPosition(pos);
            });
            //普通地图展示  
            var mapOption = {
                mapType: BMAP_NORMAL_MAP,
                maxZoom: 18,
                drawMargin: 0,
                enableFulltimeSpotClick: true,
                enableHighResolution: true
            }
            var map = new BMap.Map(normalMapContainerId, mapOption);
            var testpoint = new BMap.Point(lng, lat);
            map.centerAndZoom(testpoint, 18);
            var marker = new BMap.Marker(testpoint);
            marker.enableDragging();
            map.addOverlay(marker);
            marker.addEventListener('dragend', function (e) {
                panorama.setPosition(e.point); //拖动marker后，全景图位置也随着改变  
                panorama.setPov({ heading: -40, pitch: 6 });
            });
        }
    }
})();