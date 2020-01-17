// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function ($) {

    var $animSpeed = 200;

    $('#cleanAccordionMain > .accordion > dt:last-of-type').addClass('accordionLastDt');
    $('#cleanAccordionMain > .accordion > dd:last-of-type').addClass('accordionLastDd');
    $('#cleanAccordionMain > .accordion > dt:first-of-type').addClass('accordionFirstDt selected');
      $('#cleanAccordionMain > .accordion > dt:first-of-type').show();

    $('#cleanAccordionMain > .accordion dd').hide();
    $('#cleanAccordionMain > .dropDown1 > dd:first-of-type').slideDown($animSpeed);
    $('#cleanAccordionMain > .dropDown1 > dt:first-child > a').addClass('selected').parent().addClass('selected');
    $('#cleanAccordionMain > .accordion dt a.accordionSlide').click(function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected').parent().removeClass('selected');
            $(this).parent().next().slideUp($animSpeed);

        } else {
            $('#cleanAccordionMain > .accordion dt a.accordionSlide').removeClass('selected').parent().removeClass('selected');
            $(this).addClass('selected').parent().addClass('selected');
            $('#cleanAccordionMain > .accordion dd').slideUp($animSpeed);
            $(this).parent().next().slideDown($animSpeed);
        }
        return false;
    });

    //$('#cleanAccordionMain dd').addClass('clearfix');

});
