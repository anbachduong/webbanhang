$(window).load(function () {
    $('.product').slick({
        infinite: true,
        slidesToShow: 4,
        autoplay: true,
        autoplayspeed: 2000,
        nextArrow: '<a href="#" class="next-produce"><i class="fa fa-angle-double-right"></i></a>',
        prevArrow: '<a href="#" class="prev-produce"><i class="fa fa-angle-double-left"></i></a>',
        responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1,
                    infinite: true
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });
});
$(document).ready(function () {
    $('.search-toggle').click(function () {
        $('.sidebar-search').toggleClass('display: inline-block');
    });
});
$(window).load(function () {
    $('.promotion').slick({
        infinite: true,
        slidesToShow: 1,
        autoplay: true,
        autoplayspeed: 5000,
        nextArrow: '<a href="#" class="next-slick" style="float:right"><i class="fa fa-angle-right"></i></a>',
        prevArrow: '<a href="#" class="prev-slick" style="float:right"><i class="fa fa-angle-left"></i></a>'
    });
});