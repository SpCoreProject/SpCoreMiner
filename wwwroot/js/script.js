var $menuIcon = $('.menu__icon nav a'),
    menuContent = '.menu_content_',
    menu = '.menu_',
    index = 0;

    $menuIcon.click(function(){

      //index
      index = $(this).index();

      //Add active class and remove other elements
      $(menu + index)
            .addClass('active')
            .siblings().removeClass('active');

      //Show active element content
      $(menuContent + index)
          .addClass('active')
          .siblings().removeClass('active');

    });