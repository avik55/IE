var windowWidth= $(window).width();

// Profile Video slides
var swipervideo_perf = new Swiper('.vid_section .trending_perf', {
	slidesPerView: 3,
	spaceBetween: 20,
	nextButton: '.swiper-button-next3',
	prevButton: '.swiper-button-prev3',
	freeMode: true,
	breakpoints: {
		1024: {
			slidesPerView: 3,
			spaceBetween: 20
		},
		768: {
			slidesPerView: 2,
			spaceBetween: 20
		},
		640: {
			slidesPerView: 1,
			spaceBetween: 25
		},
		320: {
			slidesPerView: 1,
			spaceBetween: 25
		}
	}
});

// Profile Photo slides
var swiperphoto_perf = new Swiper('.photo_section .trending_perf', {
	slidesPerView: 3,
	spaceBetween: 20,
	nextButton: '.swiper-button-next4',
	prevButton: '.swiper-button-prev4',
	freeMode: true,
	breakpoints: {
		1024: {
			slidesPerView: 3,
			spaceBetween: 40
		},
		768: {
			slidesPerView: 2,
			spaceBetween: 30
		},
		640: {
			slidesPerView: 1,
			spaceBetween: 25
		},
		320: {
			slidesPerView: 1,
			spaceBetween: 25
		}
	}
});

// Similar Profile slides
var similarprofile_perf = new Swiper('.similar_pro_section .trending_slider', {
	slidesPerView: 3,
	spaceBetween: 20,
	nextButton: '.swiper-button-next1',
	prevButton: '.swiper-button-prev1',
	freeMode: true,
	breakpoints: {
		1024: {
			slidesPerView: 3,
			spaceBetween: 40
		},
		768: {
			slidesPerView: 2,
			spaceBetween: 30
		},
		640: {
			slidesPerView: 1,
			spaceBetween: 25
		},
		320: {
			slidesPerView: 1,
			spaceBetween: 25
		}
	}
});

function lessmore(obj){
		var aboutexcol = obj.parent().prev();	
		if($(aboutexcol).height() > 120 ){	
			$(aboutexcol).css('max-height', '120px');
			$('span', obj).text('Read More...');
		}else{
			$(aboutexcol).css('max-height', 'none');
			$('span', obj).text('Read Less...');
		}
}

function toggleChevron(e) {
    $(e.target)
        .prev('.panel-heading')
        .find("i.indicator")
        .toggleClass('glyphicon-chevron-down glyphicon-chevron-up');
}
$('#accordion').on('hidden.bs.collapse', toggleChevron);
$('#accordion').on('shown.bs.collapse', toggleChevron);