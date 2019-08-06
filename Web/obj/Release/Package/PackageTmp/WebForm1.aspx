<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Web.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <div class="overlay">

        <div id="alarm-dialog">

            <h2>Set alarm after</h2>

            <label class="hours">
                Hours
            <input type="number" value="0" min="0" />
            </label>

            <label class="minutes">
                Minutes
            <input type="number" value="0" min="0" />
            </label>

            <label class="seconds">
                Seconds
            <input type="number" value="0" min="0" />
            </label>

            <div class="button-holder">
                <a id="alarm-set" class="button blue">Set</a>
                <a id="alarm-clear" class="button red">Clear</a>
            </div>

            <a class="close"></a>

        </div>

    </div>

    <div class="overlay">

        <div id="time-is-up">

            <h2>Time's up!</h2>

            <div class="button-holder">
                <a class="button blue">Close</a>
            </div>

        </div>

    </div>
    <audio id="alarm-ring" preload>
        <source src="assets/audio/ticktac.mp3" type="audio/mpeg" />
        <source src="assets/audio/ticktac.ogg" type="audio/ogg" />
    </audio>
</body>

</html>
 <script type="text/javascript">  
     var pagesi = "2";//每页行数  
     var totalPage = "0";//总页数  
     var currentPage = "1";//当前页   
     $(function(){  
         $('.menu_item').click(function () {  
             $('.menu_item').removeClass('selected');  
             $(this).addClass('selected');  
         });  
                  
         $('.js_top').click(function () {  
             $('.js_top').removeClass('selected');  
             $(this).addClass('selected');  
         });  
         $('#commentList').click(function(){  
             queryForPages();  
         });  
                 
         //上一页  
         $(".page_prev").click(function(){  
             if(currentPage>1){  
                 currentPage-- ;  
                 $(".page_prev").css({display:"inline-block"});  
                 queryForPages();  
             }  
             if(currentPage==1){  
                 $(".page_prev").hide();  
             }  
             if(currentPage<totalPage){  
                 $(".page_next").css({display:"inline-block"});  
             }  
         });  
         //下一页  
         $(".page_next").click(function(){  
             if(currentPage<totalPage){  
                 currentPage++ ;  
                 $(".page_next").css({display:"inline-block"});  
                 queryForPages();  
             }  
             if(currentPage>1){  
                 $(".page_prev").css({display:"inline-block"});  
             }  
             if(currentPage==totalPage){  
                 $(".page_next").hide();  
             }  
         });   
         //跳页  
         $('.page_go').click(function(){  
             currentPage = $('.goto_area').find('input').val();  
             queryForPages();  
             if(currentPage==1){  
                 $(".page_prev").hide();  
             }  
             if(currentPage>1){  
                 $(".page_prev").css({display:"inline-block"});  
             }  
             if(currentPage<totalPage){  
                 $(".page_next").css({display:"inline-block"});  
             }  
             if(currentPage==totalPage){  
                 $(".page_next").hide();  
             }  
         });         
     });     
     function queryForPages(){  
         $.post("comment/getCommentNaichaByTimeType.do",  
          {  
              timeType:"3",  
              currentPage: currentPage,  
              pageSize: pagesi  
          },  
          function(data){    
              var good = data.countList[0].count;  
              var middle = data.countList[1].count;  
              var bad = data.countList[2].count;  
              var total = good + middle + bad;  
              $('#good').text(good);  
              $('#middle').text(middle);  
              $('#bad').text(bad);  
              $('#total').text(total);  
              totalPage = Math.ceil(total/pagesi);//总页数  
              $('#currentPage').text(currentPage);  
              $('#totalPage').text(totalPage);  
              var childhtml = '';  
              $.each(data.commentNaichaList, function(idx, obj) {  
                  var time = obj.time;  
                  var rank = obj.rank;  
                  var content = obj.content;  
                  var beCommentName = obj.beCommentName;  
                  var toCommentName = obj.toCommentName;  
                  console.log(obj.content);  
                  childhtml += '<tr>'  
                  childhtml += '<td class="table_cell js_time">'+time+'</td>';  
                  childhtml += '<td class="table_cell tr js_rank">'+rank+'</td>';  
                  childhtml += '<td class="table_cell tr js_content">'+content+'</td>';  
                  childhtml += '<td class="table_cell tr js_time">'+beCommentName+'</td>';  
                  childhtml += '<td class="table_cell tr js_time">'+toCommentName+'</td>';  
                  childhtml += '</tr>';  
              });  
              console.log(childhtml);  
              $('#js_detail').html(childhtml);  
          });  
     }   
</script>  
