jQuery(function ($) {

    'use strict', //#main-slider

    $(function () {
        $('#main-slider.carousel').carousel({
            interval: 8000
        });
    });


    $('.accordion-toggle').on('click', function () {
        $(this).closest('.panel-group').children().each(function () {
            $(this).find('>.panel-heading').removeClass('active');
        });
        $(this).closest('.panel-heading').toggleClass('active');
    });


    $(window).scroll(function () {
        var scroll = $(window).scrollTop();
        if (scroll > 1) {
            $('#header').addClass('fixed');
        } else {
            $('#header').removeClass('fixed');
        }
    });

    var form = $('#main-contact-form'); form.submit(function (event) {
        event.preventDefault();
        var form_status = $('<div class="form_status"></div>');
        $.ajax({
            url: $(this).attr('action'),
            beforeSend: function () {
                form.prepend(form_status.html('<p><i class="fa fa-spinner fa-spin"></i> Email is sending...</p>').fadeIn());
            }
        }).done(function (data) {
            form_status.html('<p class="text-success">' + data.message + '</p>').delay(3000).fadeOut();
        });
    });


    $('.gototop').click(function (event) {
        event.preventDefault();
        $('html, body').animate({
            scrollTop: $("body").offset().top
        }, 500);
    });

    // Trending Performance slides

    var swipertrending_perf = new Swiper('#trending_perf .trending_perf', {
        pagination: '.swiper-pagination',
        slidesPerView: 4,
        spaceBetween: 30,
        nextButton: '.swiper-button-next3',
        prevButton: '.swiper-button-prev3',
        freeMode: true,
        breakpoints: {
            1024: {
                slidesPerView: 4,
                spaceBetween: 40
            },
            768: {
                slidesPerView: 3,
                spaceBetween: 30
            },
            640: {
                slidesPerView: 1,
                spaceBetween: 20
            },
            320: {
                slidesPerView: 1,
                spaceBetween: 20
            }
        }
    });

    // Trending Entertainers slides

    var swipertrending = new Swiper('#trending .trending_slider', {
        slidesPerView: 4,
        paginationClickable: false,
        spaceBetween: 30,
        nextButton: '.swiper-button-next1',
        prevButton: '.swiper-button-prev1',
        freeMode: true,
        breakpoints: {
            1024: {
                slidesPerView: 4,
                spaceBetween: 40
            },
            768: {
                slidesPerView: 3,
                spaceBetween: 30
            },
            640: {
                slidesPerView: 1,
                spaceBetween: 50
            },
            320: {
                slidesPerView: 1,
                spaceBetween: 50
            }
        }
    });


    // testimonil slides

    var swipertestimonil = new Swiper('.testimonial', {
        pagination: '.swiper-pagination',
        nextButton: '.swiper-button-next2',
        prevButton: '.swiper-button-prev2',
        slidesPerView: 1,
        paginationClickable: true,
        spaceBetween: 30,
        loop: true
    });


    //range slider start
    function modifyOffset() {
        var el, newPoint, newPlace, offset, siblings, k;
        width = this.offsetWidth;
        newPoint = (this.value - this.getAttribute("min")) / (this.getAttribute("max") - this.getAttribute("min"));
        offset = -1;
        if (newPoint < 0) { newPlace = 0; }
        else if (newPoint > 1) { newPlace = width; }
        else { newPlace = width * newPoint + offset; offset -= newPoint; }
        siblings = this.parentNode.childNodes;
        for (var i = 0; i < siblings.length; i++) {
            sibling = siblings[i];
            if (sibling.id == this.id) { k = true; }
            if ((k == true) && (sibling.nodeName == "OUTPUT")) {
                outputTag = sibling;
            }
        }
        outputTag.style.left = newPlace + "px";
        outputTag.style.marginLeft = offset + "%";
        outputTag.innerHTML = this.value;
    }

    function modifyInputs() {

        var inputs = document.getElementsByTagName("input");
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].getAttribute("type") == "range") {
                inputs[i].onchange = modifyOffset;

                // the following taken from http://stackoverflow.com/questions/2856513/trigger-onchange-event-manually
                if ("fireEvent" in inputs[i]) {
                    inputs[i].fireEvent("onchange");
                } else {
                    var evt = document.createEvent("HTMLEvents");
                    evt.initEvent("change", false, true);
                    inputs[i].dispatchEvent(evt);
                }
            }
        }
    }

    modifyInputs();
    //range slider end

    function toggleChevron(e) {
        $(e.target)
            .prev('.panel-heading')
            .find("i.indicator")
            .toggleClass('glyphicon-chevron-down glyphicon-chevron-up');
    }
    $('#accordion').on('hidden.bs.collapse', toggleChevron);
    $('#accordion').on('shown.bs.collapse', toggleChevron);


    //slide toggle effect on right side search page
    var click_div = $('.trend_img .img-responsive,.trend_cnt a,.modal-overlay')
    $(click_div).click(function () {
        var hidden = $('.left_side.srchpage');
        if (hidden.hasClass('visible')) {
            $('.modal-overlay').removeClass('rightpanel');
            hidden.animate({ "right": "-1000px" }, "slow").removeClass('visible');
        } else {
            $('.modal-overlay').addClass('rightpanel');
            hidden.animate({ "right": "0px" }, "slow").addClass('visible');
        }
    });


    //View more category only for IPAD

    $('.viewmore').click(function () {
        if ($('.cat_list').hasClass('hidden-sm')) {
            $('.cat_list').removeClass('hidden-sm');
            $(this).hide(0);
        }
    })


    // animation script

    // hide our element on page load
    /*$('.trending').css('opacity', 0);
    $('.trending_perf,.cat_list,.aboutus').css('opacity', 0);

    $('.trending').waypoint(function() {
      $('.trending').addClass('slideInUp').css('opacity', 1);
    }, { offset: '70%' });
     
    $('.trending_perf').waypoint(function() {
      $('.trending_perf').addClass('slideInUp').css('opacity', 1);
    }, { offset: '70%' });
     
    $('.cat_list').waypoint(function() {
      $('.cat_list').addClass('zoomIn').css('opacity', 1);
    }, { offset: '70%' });
     
    $('.aboutus').waypoint(function() {
      $('.aboutus').addClass('fadeIn').css('opacity', 1);
    }, { offset: '70%' });
     */


    // search header


    // Remove Search if user Resets Form or hits Escape!
    $('body, #header .search button[type="reset"]').on('click keyup', function (event) {
        // console.log(event.currentTarget);
        if (event.which == 27 && $('.navbar-collapse .search').hasClass('active') ||
            $(event.currentTarget).attr('type') == 'reset') {
            closeSearch();
        }
    });


    $('body').on('click', function (e) {
        var form = $('#header .search.active');
        var drpdwn = $('.custom-dropdown, .custom-dropdown ul, .custom-dropdown li');
        if (!form.is(e.target) &&
            form.has(e.target).length === 0) {
            form.find('input').val('');
            form.removeClass('active');
        } else {

        }

        if (!drpdwn.is(e.target) &&
            drpdwn.has(e.target).length === 0) {
            drpdwn.fadeOut(200);
        } else {

        }
    });

    function closeSearch() {
        var $form = $('#header .search.active');
        $form.find('input').val('');
        $form.removeClass('active');
    }

    // Show Search if form is not active // event.preventDefault() is important, this prevents the form from submitting
    $(document).on('click', '#header .search:not(.active) button[type="submit"]', function (event) {
        event.preventDefault();
        var $form = $(this).closest('form'),
            $input = $form.find('input');
        $form.addClass('active');
        $input.focus();

    });
    // ONLY FOR DEMO // Please use $('form').submit(function(event)) to track from submission
    // if your form is ajax remember to call `closeSearch()` to close the search container
    $(document).on('click', '#header .search.active button[type="submit"]', function (event) {
        event.preventDefault();
        var $form = $(this).closest('form'),
            $input = $form.find('input');
        $('#showSearchTerm').text($input.val());
        closeSearch()
    });


    $('ul.dropdown-menu').on('click', function (event) {
        // therefore events delegated to document won't be fired
        event.stopPropagation();
    });

    var navListItems = $('div.setup-panel div a'),
          allWells = $('.setup-content'),
          allNextBtnReg = $('.nextBtnReg'),
          allNextBtn = $('.nextBtn');

    allWells.hide();

    navListItems.click(function (e) {
        e.preventDefault();
        var $target = $($(this).attr('href')),
                $item = $(this);

        if (!$item.hasClass('disabled')) {
            navListItems.addClass('btn-default donecls');
            $item.addClass('btn-primary');
            allWells.hide();
            $target.show();
            $target.find('input:eq(0)').focus();
        }
    });

    allNextBtnReg.click(function () {
        
        var hdnOTP = $(this).closest('form').find('input[name="hdnOTP"]').val();
        var enteredOTP = $(this).closest('form').find('input[name="enteredOTP"]').val();

        if (hdnOTP) {
            if (hdnOTP != "" && enteredOTP != "") {
                if (hdnOTP != enteredOTP) {
                    $(this).closest('form').find('p[id="correctOTP"]').show();
                    return false;
                }
            }           
        }

        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],input[type='number'],input[type='email'],input[type='file']"),
            isValid = true;


        // for select or dropdown list
        var curSelect = curStep.find("select");
        var curRadio = curStep.find("input[type='radio']");



        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {

            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        // for each select or droplist
        for (var i = 0; i < curSelect.length; i++) {
            var sele = "#" + curSelect[i].name;


            if ($(sele).val() == "") {
                isValid = false;
                $(curSelect[i]).closest(".form-group").addClass("has-error");
            }
        }


        // check radio button
        for (var i = 0; i < curRadio.length; i++) {

            var r = 'input[name="' + curRadio[i].name + '"]';
            var rb = $(r + ':checked').val();
            console.log(r);
            console.log(rb);
            if (rb == "") {
                isValid = false;
                $(curRadio[i]).closest(".form-group").addClass("has-error");
            }

        }


        if (isValid)
            nextStepWizard.removeAttr('disabled').trigger('click');
    });

    allNextBtn.click(function () {
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],input[type='number'],input[type='email']"),
            isValid = true;


        // for select or dropdown list
        var curSelect = curStep.find("select");
        var curRadio = curStep.find("input[type='radio']");



        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {

            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        // for each select or droplist
        for (var i = 0; i < curSelect.length; i++) {
            var sele = "#" + curSelect[i].name;


            if ($(sele).val() == "") {
                isValid = false;
                $(curSelect[i]).closest(".form-group").addClass("has-error");
            }
        }


        // check radio button
        for (var i = 0; i < curRadio.length; i++) {

            var r = 'input[name="' + curRadio[i].name + '"]';
            var rb = $(r + ':checked').val();
            console.log(r);
            console.log(rb);
            if (rb == "") {
                isValid = false;
                $(curRadio[i]).closest(".form-group").addClass("has-error");
            }

        }


        if (isValid)
            nextStepWizard.removeAttr('disabled').trigger('click');
    });

    $('div.setup-panel div a.btn-primary').trigger('click');

    //$(".step-form-2 .checkbox input").click(function(){
    //    if( $(".step-form-2 .checkbox input").is(':checked') ) {
    //	//var get_txt = $(".step-form-2 .checkbox input").next().text();


    //	var selected = $(".step-form-2 .checkbox input:checked").map(function(i,el){return el.name;}).get();

    //	 $(".step-form-2 .selected_val").text(selected.join(", "));

    //	}else{
    //		$(".step-form-2 .selected_val").text('Select');
    //	}
    //});

    $("input[name=Gender]").click(function () {
        if ($(this).is(':checked'))  // or   if( $(this).attr('checked')== true) )
            var get_txt = $(this).next().text();
        $(".selected_val").html(get_txt);
        console.log("Im In first");
    });
    $("input[name=StateId]").click(function () {
        if ($(this).is(':checked'))  // or   if( $(this).attr('checked')== true) )
            var get_txt = $(this).next().text();
        $(".selected_state").html(get_txt);
        console.log("Im In Second");
    });
    $("input[name=CatId]").click(function () {
        if ($(this).is(':checked'))  // or   if( $(this).attr('checked')== true) )
            var get_txt = $(this).next().text();
        $(".selected_catt").html(get_txt);
        console.log("Im In third");
    });

    //$("input[name=CityId]").click(function () {
    //    if ($(this).is(':checked'))  // or   if( $(this).attr('checked')== true) )
    //        var get_txt = $(this).next().text();
    //    $(".selected_city").html(get_txt);
    //    console.log("Im In forth");
    //});



    $('.chkboxdiv input[type="checkbox"]').change(function (event) {
        if ($('input[type="checkbox"]:checked').length >= 1) {
            event.preventDefault();
            $('input[type="checkbox"]').not(':checked').prop('disabled', true);
            //alert("You're not allowed to choose more than 3 Categories");
            //$('.nextBtn1.reg').hide(0);
        }
        else {
            $('input[type="checkbox"]').not(':checked').removeProp('disabled');
            //$('.nextBtn1.reg').show(0);
        }
    });

    var incre = 1;
    $('.nextBtn1.reg').click(function () {
        if ($('.section_clone').length !== 3) {
            incre++;
            $(".section_clone:first-child").clone().attr('id', 'box_sec' + incre).insertBefore('div.cat_Sec_end');
        } else {
            $('.nextBtn1.reg').hide(0);

        }
    })


    $("#slideValue").slider({
        min: 0,
        max: 2500000,
        value: [0, 1000000],
        labelledby: ['ex18-label-2a', 'ex18-label-2b']
    });


    $("#slideValue").slider();
    $("#slideValue").on("slide", function (slideEvt) {
        var minval = $('#FlbudgetMin');
        minval.val(slideEvt.value[0]);
        minval.trigger('input'); // Use for Chrome/Firefox/Edge
        minval.trigger('change'); // Use for Chrome/Firefox/Edge + IE11

        var maxval = $('#FlbudgetMax');
        maxval.val(slideEvt.value[1]);
        maxval.trigger('input'); // Use for Chrome/Firefox/Edge
        maxval.trigger('change'); // Use for Chrome/Firefox/Edge + IE11

        //$("#FlbudgetMin").attr('value', slideEvt.value[0]);
        //console.log(slideEvt);
        //console.log(slideEvt.value[0]);
        //console.log(slideEvt.value[1]);
        //$("#FlbudgetMax").attr('value', slideEvt.value[1]);
    });



    $(".filter-srch-list .scrollbar,.custom-dropdown,.form-group.drpdwn .dropdown-menu").mCustomScrollbar({
        axis: "y",
        theme: "dark-3"
    });


    var windowWidth = $(window).width();
    if (windowWidth < 800) {
        $(".filter-search-wrp").mCustomScrollbar({
            axis: "x",
            theme: "dark-3"
        });

        $("body").mCustomScrollbar();
    }


    $('.form-js-label').find('input').on('input', function (e) {
        $(e.currentTarget).attr('data-empty', !e.currentTarget.value);
    });

    $('.setup-content .control-label').on('click', function () {
        $(this).prev().focus();
    });

    $('.tab-group li').on('click', function (e) {

        e.preventDefault();

        $(this).addClass('active');
        $(this).siblings().removeClass('active');


        var tab_id = $(this).attr('data-tab');

        $('.tab-group li').removeClass('current');
        $('.tab-content_div').removeClass('current');

        $(this).addClass('current');
        console.log(tab_id);
        $("#" + tab_id).addClass('current');


    });

    $(window).on('load', function () {

        setTimeout(function () {

            $('.log-reg input').each(function () {
                $('.log-reg input').attr('value', '');
                $('.log-reg input').val('');
            });

        }, 100);
    });




    /* ---------------------------
        bluimp lighbox Gallery init
       --------------------------- */
    $('.lightbox').click(function (event) {
        event = event || window.event;
        //console.log( $(event.target).parent().hasClass('imggal') );
        if ($(event.target).parent().hasClass('imggal')) {
            var target = event.target || event.srcElement,
            link = target.src ? target.parentNode : target,
            options = {
                index: link,
                event: event,
                thumbnailIndicators: true,
                onopened: function () {
                    if (window.history && window.history.pushState) {
                        history.pushState('image gal', 'image gal', '#imggal');
                    }
                },
                onclosed: function () {
                    if (window.history && window.history.pushState) {
                        //console.log(history.state);
                        if (history.state != null) {
                            history.go(-1);
                        }
                        //$(window).trigger('popstate');
                    }
                }
            },
            links = $(this).find('a.imggal');//this.getElementsByTagName('a');          
            //window.location.href=window.location.href+'#imggal';

            blueimp.Gallery(links, options);
        }
    });

    // Lightbox video gallery call
    $("body").on("click", ".vidlink", function (event) {
        $("#blueimp-gallery").addClass("vid-gallery");
        event.preventDefault();
        event = event || window.event;
        var _html = '<div class="iframe-wrap"><iframe class="vid-iframe" src="' + $(this).attr('href') + '" frameborder="0"></iframe></div>';
        //window.location.href=window.location.href+'#vidgal';
        var youtubegal = blueimp.Gallery($(this), {
            onopen: function () {
            },
            onopened: function () {
                setTimeout(function () {
                    $(".vid-gallery .slides .slide").html('');
                    $(".vid-gallery .slides .slide").html(_html);
                    $(".blueimp-gallery .vid-gallery .slides .slide > img").remove();
                    $('.blueimp-gallery .vid-gallery .close').fadeIn();
                    setMiddle();
                    $('body,html').css("overflow", "hidden");
                }, 200);
                if (window.history && window.history.pushState) {
                    history.pushState('vid gal', 'image gal', '#vidgal');
                }
            },
            onclosed: function () {
                $(".vid-gallery .slides .slide").html('');
                $("#blueimp-gallery").removeClass("vid-gallery");
                $('.blueimp-gallery .vid-gallery .close').fadeOut(300);
                $('body,html').css("overflow", "auto");
                if (window.history && window.history.pushState) {
                    if (history.state != null) {
                        history.go(-1);
                    }
                }
                //console.log('vid close');
            }
        });

    });



    function setMiddle() {
        var vidgalFrame = $(".blueimp-gallery.vid-gallery .vid-iframe"),
            vH = vidgalFrame.height();
        if ($(window).height() > 400) {
            $(".blueimp-gallery.vid-gallery .vid-iframe").css({
                "top": "50%",
                "margin-top": -(vH / 2) + 'px'
            });
        } else {
            $(".blueimp-gallery.vid-gallery .vid-iframe").css({
                "top": "0",
                "margin-top": '0'
            });
        }
    }

});


//Moksha Script
