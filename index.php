<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Aottg 2</title>
    <?php
      include("website/includes/imports.php");
    ?>
    <link rel="shortcut icon" href="website/resources/logo.png" type="image/x-icon">
</head>
<body>
    <?php
      include("website/includes/navbar.php");
    ?>

    <nav class="dottedNavbar">
      <ul>
        <li>
          <a href="#home" class="dot active" data-scroll="home"><span>Home</span></a>
        </li>
        <li>
          <a href="#features" class="dot" data-scroll="features"><span>Features</span></a>
        </li>
        <!--<li>
          <a href="#team" class="dot" data-scroll="team"><span>Team</span></a>
        </li>-->
      </ul>
    </nav>

    <section id="home" class="user-select-none sec">
      <h1>Aottg <span style="color: rgb(34, 202, 214) !important;">2<span></h1>
      <button type="button" class="btn btn-outline-light baixar">Download</button>
    </section>
    
    <?php
      include("website/features.php");
    ?>

  <!--<div class="team user-select-none sec" id="team">
    <div class="container">
        <p class="row">
          <button class="btn btn-outline-light btn-team col-2" type="button" data-bs-toggle="collapse" data-bs-target="#teamCollapse" aria-expanded="false" aria-controls="teamCollapse">
            Meet the team
          </button>
        </p>
        <div class="collapse" id="teamCollapse">
          <div class="card card-body">
            <?php
              include("includes/team.php");
            ?>
          </div>
        </div>
      </div>
    </div>-->

    <?php
      include("website/includes/footer.php");
    ?>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script type="text/javascript">
    
      $(document).ready(function(){

        $('.collapse').on('shown.bs.collapse', function(){
          document.getElementById("team").style.height = "600px";
        });
        $('.collapse').on('hidden.bs.collapse', function(){
          document.getElementById("team").style.height = "200px";
        });
        
        $(window).on('scroll', function(){

          var link = $('.dottedNavbar a.dot');
          var top = $(window).scrollTop();

          $('.sec').each(function(){
            var id = $(this).attr('id');
            var height = $(this).height();
            var offset = $(this).offset().top - 150;
            if(top >= offset && top < offset + height){
              link.removeClass('active');
              $('.dottedNavbar').find('[data-scroll="' + id + '"]').addClass('active');
            }
          });
          
        });

      });

    </script>
</body>
</html>