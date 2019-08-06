$(document).ready(function () {
    $(".Menu ul li a").click(function () {
        var checkElement = $(this).next();
        if (checkElement.is("ul") && checkElement.is(":visible")) {
            checkElement.slideUp("normal");
            return false;
        }
        if (checkElement.is("ul") && checkElement.is(":hidden")) {
            $(".Menu ul li ul:visible").slideUp("normal");
            checkElement.slideDown("normal");
            return false;
        }
    });
    var CkMenu = $(".Menu a[href*='" + pageName() + "']");
    $(CkMenu).parent("li").parent("ul").css({ display: "block" })
    $(CkMenu).addClass("CkMenu");
});

String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.ltrim = function () {
    return this.replace(/(^\s*)/g, "");
}
String.prototype.rtrim = function () {
    return this.replace(/(\s*$)/g, "");
}


// 取当前页面名称(不带后缀名)
function pageName() {
    var a = location.href;
    var b = a.split("/");
    var c = b.slice(b.length - 1, b.length).toString(String).split(".");
    return c.slice(0, 1);
}

//补零  num:数据 n:长度
function pad(num, n) {
    var len = num.toString().length;
    while (len < n) {
        num = "0" + num;
        len++;
    }
    return num;
}


function formatTime(val) {
    var re = /-?\d+/;
    var m = re.exec(val);
    var d = new Date(parseInt(m[0]));
    return d;
}
Date.prototype.format = function (format) {
    var it = new Date();
    var it = this;
    var M = it.getMonth() + 1, H = it.getHours(), m = it.getMinutes(), d = it.getDate(), s = it.getSeconds();
    var n = {
        'yyyy': it.getFullYear()
    , 'MM': pad(M, 2), 'M': M
    , 'dd': pad(d, 2), 'd': d
    , 'HH': pad(H, 2), 'H': H
    , 'mm': pad(m, 2), 'm': m
    , 'ss': pad(s, 2), 's': s
    };
    return format.replace(/([a-zA-Z]+)/g, function (s, $1) { return n[$1]; });
}