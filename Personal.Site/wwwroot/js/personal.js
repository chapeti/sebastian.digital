$(function(){

    /*-------------------------------------------------------------------*/
    /*  1. Preloader. Requires jQuery jpreloader plugin.
    /*-------------------------------------------------------------------*/
    $(document).ready(function() {
        if ($.fn.jpreLoader){
            $('body').jpreLoader({
                showPercentage: false,
                loaderVPos: '50%'
            });
        }
    });
    

    /*-------------------------------------------------------------------*/
    /*  2. Makes the height of all selected elements (".match-height")
    /*  exactly equal. Requires jQuery matchHeight plugin.
    /*-------------------------------------------------------------------*/
    $(window).smartload(function(){
        if ($.fn.matchHeight){
            $('.match-height').matchHeight();
        }
    });
    
    
    /*-------------------------------------------------------------------*/
    /*  3. Full screen
    /*-------------------------------------------------------------------*/
    function setResizeContent() {
        var minHeight = $(window).height();
        $('.full-screen').css('min-height', minHeight);
    }
    
    setResizeContent();

    $(window).smartresize(function(){
        setResizeContent();
    });

    
    /*-------------------------------------------------------------------*/
    /*  4. Just did another hack of dropdown menu for Bootstrap scrollspy.
    /*-------------------------------------------------------------------*/
    $('body').on('activate.bs.scrollspy', function(){
        $('.page-scroll.dropdown > .dropdown-toggle').each(function(){
            $(this).attr('data-target', '#');
        });
    });
    
    
    /*-------------------------------------------------------------------*/
    /*  5. Page scrolling feature, requires jQuery Easing plugin.
    /*-------------------------------------------------------------------*/
    var pageScroll = function(){
        $('.page-scroll > a').bind('click', function(e){
            e.preventDefault();
            
            var anchor = $(this),
            href = anchor.attr('href'),
            offset = $('body').attr('data-offset');
            
            $('html, body').stop().animate({
                scrollTop: $(href).offset().top - (offset - 1)
            }, 1500, 'easeInOutExpo');
            
            /*
             * Automatically retract the navigation after clicking 
             * on one of the menu items.
             */
            if(!$(this).parent().hasClass('dropdown')){
                $('.berg-collapse').collapse('hide');
            }
        });
    };
    
    pageScroll();
    
    
    /*-------------------------------------------------------------------*/
    /*  6. Make navigation menu on your page always stay visible.
    /*  Requires jQuery Sticky plugin.
    /*-------------------------------------------------------------------*/
    var stickyMenu = function(){
        var ww = Math.max($(window).width(), window.innerWidth),
        nav = $('.navbar.navbar-fixed-top');

        if ($.fn.unstick){
            nav.unstick();
        }
        
        if ($.fn.sticky && ww >= 992){
            nav.sticky({topSpacing: 0});
        }
    };
    
    stickyMenu();
    
    // Call stickyMenu() when window is resized.
    $(window).smartresize(function(){
        stickyMenu();
    });
    
    
    /*-------------------------------------------------------------------*/
    /*  7. Navbar dropdown opening on hover,
    /*  and opening on click for collapsed navbar.
    /*-------------------------------------------------------------------*/
    var toggleNavbarMethod = function(){
        var ww = Math.max($(window).width(), window.innerWidth),
        dropdown = $('.navbar .dropdown');
        
        if (ww >= 992){
            dropdown.on('mouseover', function(){
                if (!$(this).hasClass('open')){
                    $(this).addClass('open');
                }
            }).on('mouseout', function(){
                if ($(this).hasClass('open')){
                    $(this).removeClass('open');
                }
            });
        }
        else {
            dropdown.off('mouseover').off('mouseout');
        }
    };
    
    toggleNavbarMethod();
    
    // Call toggleNavbarMethod(); when window is resized.
    $(window).smartresize(function(){
        toggleNavbarMethod();
    });
    
    
    /*-------------------------------------------------------------------*/
    /*  8. Prevent bootstrap dropdown closing when clicked.
    /*-------------------------------------------------------------------*/
    $('.dropdown-menu').click(function(e){
        e.stopPropagation();
    });
    
 
    /*-------------------------------------------------------------------*/
    /*  9. Portfolio gallery. Requires jQuery Magnific Popup plugin.
    /*-------------------------------------------------------------------*/
    if ($.fn.magnificPopup) {

        $('.portfolio-youtube').magnificPopup({
            delegate: 'a.zoom',
            type: 'iframe',
            mainClass: 'mfp-fade',
            removalDelay: 160,
            preloader: false,

            fixedContentPos: false
        });

        $.extend(true, $.magnificPopup.defaults, {
            iframe: {
                patterns: {
                    youtube: {
                        index: 'youtube.com/',
                        id: 'v=',
                        src: 'http://www.youtube.com/embed/%id%?autoplay=1'
                    }
                }
            }
        });

        $('.popup-modal').magnificPopup({
            type: 'inline',
            preloader: false,
            focus: '#username',
            modal: true
        });
        $(document).on('click', '.popup-modal-dismiss', function (e) {
            e.preventDefault();
            $.magnificPopup.close();
        });


        $('.portfolio:not(.portfolio-youtube)').magnificPopup({
            delegate: 'a.zoom',
            type: 'image',
            fixedContentPos: false,

            // Delay in milliseconds before popup is removed
            removalDelay: 300,

            // Class that is added to popup wrapper and background
            mainClass: 'mfp-fade',

            gallery: {
                enabled: false,
                preload: [0,2],
                arrowMarkup: '<button title="%title%" type="button" class="mfp-arrow mfp-arrow-%dir%"></button>',
                tPrev: 'Previous',
                tNext: 'Next'
            }            
        });

    }
    
    
    /*-------------------------------------------------------------------*/
    /*  10. Column Chart (Section - My Strengths)
    /*-------------------------------------------------------------------*/
    var columnChart = function (){
        $('.column-chart').find('.item-progress').each(function(){
            var itemProgress = $(this);
            var itemProgressHeight = $(this).parent().height() * ($(this).data('percent') / 100);
            
            itemProgress.css('height', itemProgressHeight);
        });
    };
    
    // Call columnChart() when window is loaded.
    $(window).smartload(function(){
        columnChart();
    });
    
    
    /*-------------------------------------------------------------------*/
    /*  11. Section - My Resume
    /*-------------------------------------------------------------------*/
    var resumeCollapse = function (){
        var ww = Math.max($(window).width(), window.innerWidth),
        workItem = $('.collapse', '#work'),
        educationItem = $('.collapse', '#education');
        
        if (ww < 768){
            workItem.collapse('show');
            educationItem.collapse('show');
        }
        else{
            workItem.not(':first').collapse('hide');
            educationItem.not(':first').collapse('hide');
        }
    };
    
    // Call resumeCollapse() when window is loaded.
    $(window).smartload(function(){
        resumeCollapse();
    });
    
    // Call resumeCollapse() when window is resized.
    $(window).smartresize(function(){
        resumeCollapse();
    });
    
    
    /*-------------------------------------------------------------------*/
    /*	12. References slider. Requires Flexslider plugin.
    /*-------------------------------------------------------------------*/
    $(window).smartload(function(){
        if ($.fn.flexslider){
            var flex = $('.flexslider.references');
    
            flex.flexslider({
                selector: ".slides > .item",
                manualControls: ".flex-control-nav li",
                directionNav : false,
                slideshowSpeed: 4000,
                after: function(slider){
                    if (!slider.playing) {
                        slider.play();
                    }
                }
            }); 
        }
    });
    
    $('a.flex-prev').on('click', function(e){
        e.preventDefault();
        $('.flexslider').flexslider('prev');
    });
    
    $('a.flex-next').on('click', function(e){
        e.preventDefault();
        $('.flexslider').flexslider('next');
    });
    
    
    /*-------------------------------------------------------------------*/
    /*  13. Circle Chart (Section - Skills & Expertise)
    /*-------------------------------------------------------------------*/
    var circleChart = function (){
        $('.circle-chart').find('.item-progress').each(function(){
            var item = $(this),
            maxHeight = 108,
            newHeight = maxHeight * ($(this).data('percent') / 100);
            
            // Only animate elements when using non-mobile devices    
            if (jQuery.browser.mobile === false){
                item.one('inview', function(isInView) {
                    if (isInView){
                        // Animate item
                        item.animate({
                            height: newHeight
                        },1500);
                    }
                });
            }
            else{
                item.css('height', newHeight);
            }
        });
    };
    
    // Call circleChart() when window is loaded.
    $(window).smartload(function(){
        circleChart();
    });
    
    
    /*-------------------------------------------------------------------*/
    /*  14. Bar Chart (Section - Knowledge)
    /*-------------------------------------------------------------------*/
    var barChart = function (){
        $('.bar-chart').find('.item-progress').each(function(){
            var item = $(this),
            percent = $(this).prev(),
            newWidth = $(this).parent().width() * ($(this).data('percent') / 100);
            
            // Only animate elements when using non-mobile devices    
            if (jQuery.browser.mobile === false){
                item.one('inview', function(isInView) {
                    if (isInView){
                        // Animate item
                        item.animate({
                            width: newWidth
                        },1500);
                        
                        percent.animate({
                            left: newWidth - percent.width()
                        },1500);
                    }
                });
            }
            else{
                item.css('width', newWidth);
                percent.css('left', newWidth - percent.width());
            }
        });
    };
    
    // Call barChart() when window is loaded.
    $(window).smartload(function(){
        barChart();
    });
    
    // Call barChart() when window is resized.
    $(window).smartresize(function(){
        barChart();
    });
    
    
    /*-------------------------------------------------------------------*/
    /*  15. Milestones counter.
    /*-------------------------------------------------------------------*/
    var counter = function (){
        var number = $('.milestones').find('.number');
        
        if ($.fn.countTo){
            number.countTo({
                speed: 3000
            });
        }
    };
    
    if (jQuery.browser.mobile === false){
        var number = $('.milestones .number');
        
        number.one('inview', function(isInView) {
            if (isInView){
                counter();
            }
        });
    }
    else{
        counter();
    }
    

    /*-------------------------------------------------------------------*/
    /*  16. Load images lazy.
    /*-------------------------------------------------------------------*/
   const defaults = {
      imageLoadedClass: 'js-lazy-image--handled',
      imageSelector: '.img-lazy-load',
      // If the image gets within 50px in the Y axis, start the download.
      rootMargin: '50px 0px',
      threshold: 0.01
    };

    let config,
        images,
        imageCount,
        observer;

    function fetchImage(url) {
      return new Promise((resolve, reject) => {
        const image = new Image();
        image.src = url;
        image.onload = resolve;
        image.onerror = reject;
      });
    }

    function preloadImage(image) {
      const src = image.dataset.src;
      if (!src) {
        return;
      }

      return fetchImage(src).then(() => {
          applyImage(image, src);

          var isPortfolioImage = $(image).hasClass("portfolioImage");

          if (isPortfolioImage) {
              initializeFilterablePortfolio();
          }
          
      });
    }

    function loadImagesImmediately(images) {
      // foreach() is not supported in IE
      for (let i = 0; i < images.length; i++) {
        let image = images[i];
        preloadImage(image);
      }
    }

    function disconnect() {
      if (!observer) {
        return;
      }
      observer.disconnect();
    }

    function onIntersection(entries) {
      // Disconnect if we've already loaded all of the images
      if (imageCount === 0) {
        disconnect();
        return;
      }

      // Loop through the entries
      for (let i = 0; i < entries.length; i++) {
        let entry = entries[i];
        // Are we in viewport?
        if (entry.intersectionRatio > 0) {
          imageCount--;

          // Stop watching and load the image
          observer.unobserve(entry.target);
          preloadImage(entry.target);
        }
      }
    }

    function applyImage(img, src) {
      // Prevent this from being lazy loaded a second time.
      img.classList.add(config.imageLoadedClass);
      img.src = src;
    }

    let LazyLoad = {

      init: (options) => {
        config = {...defaults, ...options};

        images = document.querySelectorAll(config.imageSelector);
        imageCount = images.length;

        // If we don't have support for intersection observer, loads the images immediately
        if (!('IntersectionObserver' in window)) {
          loadImagesImmediately(images);
        } else {
          // It is supported, load the images
          observer = new IntersectionObserver(onIntersection, config);

          // foreach() is not supported in IE
          for (let i = 0; i < images.length; i++) {
            let image = images[i];
            if (image.classList.contains(config.imageLoadedClass)) {
              continue;
            }

            observer.observe(image);
          }
        }
      }
    };

    LazyLoad.init();

});