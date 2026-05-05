<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="KendaWeb.Home" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <%-- <div style="background-image:url('../image/bachground.jpg');background-size:cover; height:94vh; width:100%;">
    </div>--%>








<%--<%--    tet--%>
<!-- Chỉ áp dụng cho trang này: Countdown Tết Âm lịch 2026 + Cánh hoa rơi -->

<%--<style>
    @import url("https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,300;0,400;0,500;0,600;0,900;1,100&display=swap");

    /* Reset cục bộ để không ảnh hưởng toàn site */
    .tet-container * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
    }

    .tet-container {
        font-family: "Poppins", sans-serif;
        position: relative;
        min-height: 100vh;
        background:
            url('../image/tet1.jpg')
            no-repeat center center fixed;
        background-size: cover;
        background-attachment: fixed;
        background-blend-mode: luminosity;
        overflow: hidden;
    }

    .tet-content {
        position: absolute;
        inset: 80px;
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
        z-index: 10;
        box-shadow: 0 50px 50px rgba(0, 0, 0, 0.9), 0 0 0 100px rgba(0, 0, 0, 0.1);
        pointer-events: none; /* Cho phép click xuyên qua nếu có link khác */
    }

    .tet-content h2 {
        text-align: center;
        font-size: 4em;
        font-weight: 600;
        line-height: 0.7em;
        color: #f8f8f8;
        margin-top: -80px;
        pointer-events: auto;
    }

    .tet-content h2 span {
        display: block;
        font-weight: 300;
        letter-spacing: 6px;
        font-size: 0.2em;
        color: #fff;
    }

    .countdown {
        display: flex;
        margin-top: 50px;
        pointer-events: auto;
    }

    .countdown div {
        position: relative;
        width: 100px;
        height: 100px;
        line-height: 100px;
        text-align: center;
        background: #000000;
        color: #fff;
        margin: 0 15px;
        font-size: 3em;
        font-weight: 500;
        border-radius: 12px;
    }

    .countdown div:before {
        content: "";
        position: absolute;
        bottom: -30px;
        left: 0;
        width: 100%;
        height: 35px;
        background: #ffce00;
        color: #333;
        font-size: 0.35em;
        line-height: 35px;
        font-weight: 300;
        border-radius: 0 0 12px 12px;
    }

    .countdown div#day:before    { content: "Ngày"; }
    .countdown div#hour:before   { content: "Giờ"; }
    .countdown div#minute:before { content: "Phút"; }
    .countdown div#second:before { content: "Giây"; }

    /* Cánh hoa rơi */
    .falling-leaves {
        position: fixed;
        inset: 0;
        z-index: 5;                     /* Nằm trên background nhưng dưới nội dung chính nếu cần */
        pointer-events: none;
        overflow: hidden;
    }

    .leaf-scene {
        position: absolute;
        inset: 0;
        width: 100%;
        height: 100%;
        transform-style: preserve-3d;
        perspective: 900px;
    }

    .leaf-scene div {
        position: absolute;
        width: 24px;
        height: 24px;
        background: url('../SVG/canh-hoa-mai.svg') no-repeat center/contain;
        backface-visibility: visible;
    }

    .leaf-scene div:nth-child(2n) {
        background: url('../SVG/canh-hoa-dao.svg') no-repeat center/contain;
    }
</style>

<!-- Wrapper cục bộ -->
<div class="tet-container">

    <!-- Cánh hoa rơi -->
    <div class="falling-leaves">
        <div class="leaf-scene"></div>
    </div>

    <!-- Nội dung countdown -->
    <div class="tet-content">
        <h2><span>Countdown to Tết Âm Lịch</span> Bính Ngọ 2026</h2>
        <div class="countdown">
            <div id="day">na</div>
            <div id="hour">na</div>
            <div id="minute">na</div>
            <div id="second">na</div>
        </div>
    </div>

</div>

<script>
    // Countdown đến mùng 1 Tết 2026 (17/02/2026 00:00:00)
    var countDate = new Date("Feb 17, 2026 00:00:00").getTime();

    function updateCountdown() {
        var now = new Date().getTime();
        var gap = countDate - now;

        if (gap <= 0) {
            document.querySelector(".countdown").innerHTML =
                "<h3 style='color:#ffce00; font-size:2em; margin-top:20px;'>Chúc Mừng Năm Mới Bính Ngọ 2026!</h3>";
            return;
        }

        var second = 1000;
        var minute = second * 60;
        var hour = minute * 60;
        var day = hour * 24;

        var d = Math.floor(gap / day);
        var h = Math.floor((gap % day) / hour);
        var m = Math.floor((gap % hour) / minute);
        var s = Math.floor((gap % minute) / second);

        document.getElementById("day").innerText = d < 10 ? "0" + d : d;
        document.getElementById("hour").innerText = h < 10 ? "0" + h : h;
        document.getElementById("minute").innerText = m < 10 ? "0" + m : m;
        document.getElementById("second").innerText = s < 10 ? "0" + s : s;
    }

    setInterval(updateCountdown, 1000);
    updateCountdown(); // Chạy ngay lần đầu

    // Hiệu ứng cánh hoa (tinh chỉnh nhẹ, chạy độc lập)
    var LeafScene = function (viewport) {
        this.viewport = viewport;
        this.world = document.createElement("div");
        this.leaves = [];
        this.options = {
            numLeaves: 28,
            wind: { magnitude: 1.3, maxSpeed: 8, duration: 120, start: 0, speed: 0 }
        };
        this.width = this.viewport.offsetWidth;
        this.height = this.viewport.offsetHeight;
        this.timer = 0;

        this._resetLeaf = function (leaf) {
            leaf.x = 2 * this.width - Math.random() * this.width * 1.8;
            leaf.y = -30;
            leaf.z = 200 * Math.random() + 50;
            if (leaf.x > this.width) {
                leaf.x = this.width + 20;
                leaf.y = Math.random() * this.height / 2;
            }
            if (this.timer === 0) leaf.y = Math.random() * this.height;

            leaf.rotation = {
                axis: Math.random() > 0.5 ? "X" : "Z",
                value: Math.random() * 360,
                speed: 6 + Math.random() * 10
            };
            leaf.xSpeedVariation = 0.7 * Math.random() - 0.35;
            leaf.ySpeed = 1.4 + Math.random() * 2;
            return leaf;
        };

        this._updateLeaf = function (leaf) {
            var wind = this.options.wind.speed(this.timer - this.options.wind.start, leaf.y) + leaf.xSpeedVariation;
            leaf.x -= wind;
            leaf.y += leaf.ySpeed;
            leaf.rotation.value += leaf.rotation.speed;

            var transform =
                "translateX(" + leaf.x + "px) translateY(" + leaf.y + "px) translateZ(" + leaf.z + "px) " +
                "rotate" + leaf.rotation.axis + "(" + leaf.rotation.value + "deg)";

            if (leaf.rotation.axis !== "X") {
                transform += " rotateX(" + (leaf.rotation.value * 0.5) + "deg)";
            }

            leaf.el.style.transform = transform;
            leaf.el.style.webkitTransform = transform;
            leaf.el.style.MozTransform = transform;
            leaf.el.style.oTransform = transform;

            if (leaf.x < -50 || leaf.y > this.height + 50) this._resetLeaf(leaf);
        };

        this._updateWind = function () {
            if (this.timer === 0 || this.timer > this.options.wind.start + this.options.wind.duration) {
                this.options.wind.magnitude = Math.random() * this.options.wind.maxSpeed + 1;
                this.options.wind.duration = 60 * this.options.wind.magnitude + (Math.random() * 40 - 20);
                this.options.wind.start = this.timer;

                var h = this.height;
                this.options.wind.speed = function (t, y) {
                    var intensity = (this.magnitude / 2) * (h - (2 * y) / 3) / h;
                    return intensity * Math.sin((2 * Math.PI / this.duration) * t + (3 * Math.PI / 2)) + intensity;
                };
            }
        };
    };

    LeafScene.prototype.init = function () {
        for (var i = 0; i < this.options.numLeaves; i++) {
            var leaf = {
                el: document.createElement("div"),
                x: 0, y: 0, z: 0,
                rotation: { axis: "X", value: 0, speed: 0 },
                xSpeedVariation: 0,
                ySpeed: 0
            };
            this._resetLeaf(leaf);
            this.leaves.push(leaf);
            this.world.appendChild(leaf.el);
        }
        this.world.className = "leaf-scene";
        this.viewport.appendChild(this.world);

        this.world.style.perspective = "900px";
        this.world.style.webkitPerspective = "900px";
        this.world.style.MozPerspective = "900px";

        window.addEventListener("resize", () => {
            this.width = this.viewport.offsetWidth;
            this.height = this.viewport.offsetHeight;
        });
    };

    LeafScene.prototype.render = function () {
        this._updateWind();
        this.leaves.forEach(leaf => this._updateLeaf(leaf));
        this.timer++;
        requestAnimationFrame(this.render.bind(this));
    };

    var leafContainer = document.querySelector(".falling-leaves");
    if (leafContainer) {
        var scene = new LeafScene(leafContainer);
        scene.init();
        scene.render();
    }
</script>--%>


<%--    noel--%>
    <%--<style>    

   
        body {
           
            width: 100%;
            background-image:url('../image/28577.jpg');background-size:cover;
          
            overflow: hidden;
        }

        .container {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        #days {
            font-size: 50px;
            color: #FFF;
            text-align: center;
            letter-spacing: 3px;
        }

        .drop {
            position: absolute;
            top: 0;
            z-index: -1;
            opacity: 0;
        }

        .snow {
            height: 8px;
            width: 8px;
            border-radius: 100%;
            background-color: #FFF;
            box-shadow: 0 0 10px #FFF
        }


        .animate {
            animation: falling 8.5s infinite ease-in;
        }


        @keyframes falling {
            0% {
                top: 0;
                opacity: 1;
            }

            100% {
                top: 1500px;
                opacity: 0
            }
        }
    </style>
      

    <div class="container"  >

        <p id="days" style="color:black;font-family:Jokerman"></p>

    </div>
    <script>
        
        let today = new Date();

        let christmasYear = today.getFullYear();

    
        if (today.getMonth() == 11 &&
            today.getDate() > 25) {

      
            christmasYear = christmasYear + 1;
        }

        
        let christmasDate =
            new Date(christmasYear, 11, 25);

       
        let dayMilliseconds =
            1000 * 60 * 60 * 24;

        let remainingDays = Math.ceil(
            (christmasDate.getTime() - today.getTime()) /
            (dayMilliseconds)
        );



        if (remainingDays == 0)
            $('#days').text("It's Christmas!! Merry Christmas!");

        if (remainingDays < 0)
            $('#days').text("Christmas was " + -1 * (remainingDays) + " days ago.");

        if (remainingDays > 0)
            $('#days').text(remainingDays + " days to Christmas!");
          
        

       
        snowDrop(150, randomInt(1440, 1280));
        snow(150, 150);

        function snow(num, speed) {
            if (num > 0) {
                setTimeout(function () {
                    $('#drop_' + randomInt(1, 250)).addClass('animate');
                    num--;
                    snow(num, speed);
                }, speed);
            }
        };

        function snowDrop(num, position) {
            if (num > 0) {
                var drop = '<div class="drop snow" id="drop_' + num + '" ></div>';

                $('body').append(drop);
                $('#drop_' + num).css('left', position);
                num--;
                snowDrop(num, randomInt(60, 2080));
            }
        };

        function randomInt(min, max) {
            return Math.floor(Math.random() * (max - min + 1) + min);
        };
    </script>--%>




    <!-- Background ảnh chính của trang Home -->
<div style="background-image: url('../image/1231111.jpg'); 
            background-size: cover; 
            background-position: center; 
            height: 94vh; 
            width: 100%; 
            position: relative;">

    <!-- Layer hiệu ứng cánh hoa rơi (nằm trên background nhưng không che tương tác) -->
    <div class="falling-leaves">
        <div class="leaf-scene"></div>
    </div>

</div>

<!-- CSS cho hiệu ứng cánh hoa rơi -->
<style type="text/css">
    .falling-leaves {
        position: absolute;
        inset: 0;
        z-index: 5;                     /* Cao hơn background, nhưng thấp hơn navbar nếu cần */
        pointer-events: none;           /* Không cản click chuột lên các phần tử khác */
        overflow: hidden;
    }

    .leaf-scene {
        position: absolute;
        inset: 0;
        width: 100%;
        height: 100%;
        transform-style: preserve-3d;
        perspective: 900px;
    }

    .leaf-scene div {
        position: absolute;
        width: 28px;
        height: 28px;
        background: url('../SVG/canh-hoa-mai.svg') no-repeat center/contain;
        backface-visibility: visible;
    }

    .leaf-scene div:nth-child(2n) {
        background: url('../SVG/canh-hoa-dao.svg') no-repeat center/contain;
    }
</style>

<!-- JavaScript hiệu ứng cánh hoa rơi -->
<script type="text/javascript">
    var LeafScene = function (viewport) {
        this.viewport = viewport;
        this.world = document.createElement("div");
        this.leaves = [];
        this.options = {
            numLeaves: 35,                  // Số lượng cánh hoa - điều chỉnh nếu muốn dày/mỏng hơn
            wind: {
                magnitude: 1.4,
                maxSpeed: 9,
                duration: 140,
                start: 0,
                speed: 0
            }
        };
        this.width = this.viewport.offsetWidth;
        this.height = this.viewport.offsetHeight;
        this.timer = 0;

        this._resetLeaf = function (leaf) {
            leaf.x = Math.random() * this.width * 1.5 + this.width * 0.5;
            leaf.y = -50;
            leaf.z = Math.random() * 300 + 100;
            leaf.rotation = {
                axis: Math.random() > 0.5 ? "X" : "Z",
                value: Math.random() * 360,
                speed: 4 + Math.random() * 12
            };
            leaf.xSpeedVariation = Math.random() * 1.2 - 0.6;
            leaf.ySpeed = 1.5 + Math.random() * 2.5;
            return leaf;
        };

        this._updateLeaf = function (leaf) {
            var wind = this.options.wind.speed(this.timer - this.options.wind.start, leaf.y) + leaf.xSpeedVariation;
            leaf.x -= wind * 0.8;
            leaf.y += leaf.ySpeed;
            leaf.rotation.value += leaf.rotation.speed;

            var t =
                `translateX(${leaf.x}px) translateY(${leaf.y}px) translateZ(${leaf.z}px) ` +
                `rotate${leaf.rotation.axis}(${leaf.rotation.value}deg)`;

            if (leaf.rotation.axis !== "X") {
                t += ` rotateX(${leaf.rotation.value * 0.6}deg)`;
            }

            leaf.el.style.transform = t;
            leaf.el.style.webkitTransform = t;

            if (leaf.x < -80 || leaf.y > this.height + 80) {
                this._resetLeaf(leaf);
            }
        };

        this._updateWind = function () {
            if (this.timer === 0 || this.timer > this.options.wind.start + this.options.wind.duration) {
                this.options.wind.magnitude = Math.random() * this.options.wind.maxSpeed + 2;
                this.options.wind.duration = 80 * this.options.wind.magnitude + (Math.random() * 60 - 30);
                this.options.wind.start = this.timer;

                var h = this.height;
                this.options.wind.speed = function (t, y) {
                    var intensity = (this.magnitude / 2) * (h - (2 * y) / 3) / h;
                    return intensity * (Math.sin((2 * Math.PI / this.duration) * t + Math.PI * 1.5) + 1);
                };
            }
        };
    };

    LeafScene.prototype.init = function () {
        for (var i = 0; i < this.options.numLeaves; i++) {
            var leaf = {
                el: document.createElement("div"),
                x: 0, y: 0, z: 0,
                rotation: { axis: "X", value: 0, speed: 0 },
                xSpeedVariation: 0,
                ySpeed: 0
            };
            this._resetLeaf(leaf);
            this.leaves.push(leaf);
            this.world.appendChild(leaf.el);
        }
        this.world.className = "leaf-scene";
        this.viewport.appendChild(this.world);

        window.addEventListener("resize", () => {
            this.width = this.viewport.offsetWidth;
            this.height = this.viewport.offsetHeight;
        });
    };

    LeafScene.prototype.render = function () {
        this._updateWind();
        this.leaves.forEach(leaf => this._updateLeaf(leaf));
        this.timer++;
        requestAnimationFrame(this.render.bind(this));
    };

    // Khởi động hiệu ứng khi trang load
    window.addEventListener("load", function () {
        var container = document.querySelector(".falling-leaves");
        if (container) {
            var scene = new LeafScene(container);
            scene.init();
            scene.render();
        }
    });
</script>
</asp:Content>
