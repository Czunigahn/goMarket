/**
*    Morph gallery for jQuery 
*    Copyright (C) 2008 Ziadin Givan www.CodeAssembly.com  
*
*    This program is free software: you can redistribute it and/or modify
*    it under the terms of the GNU Lesser General Public License as published by
*    the Free Software Foundation, either version 3 of the License, or
*    (at your option) any later version.
*
*    This program is distributed in the hope that it will be useful,
*    but WITHOUT ANY WARRANTY; without even the implied warranty of
*    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*    GNU General Public License for more details.
*
*    You should have received a copy of the GNU Lesser General Public License
*    along with this program.  If not, see http://www.gnu.org/licenses/
*    
*    Examples 
*	 $(".jmorph").jmorphGallery();
*/
jQuery.fn.jmorphGallery = function(settings) 
{
	return this.each( function()//do it for each matched element
	{
		
		settings = jQuery.extend(//provide default settings
		{
			timeout: 1000
		} , settings);

		var parent = $(this);
		var images = new Array;
		var currentImageNr = 0;
		var preloadImage = new Image();

		//get all images
		$(".band li a", parent).each(function(i)
		{
			images.push($(this).attr('href'));
		});
		
		//load first image
		preloadImage.src = images[currentImageNr];
		$(preloadImage).bind('load',function ()
		{
			$('.nextImg',parent).css('background-image','url(' + images[currentImageNr] + ')').
			width(preloadImage.width).
			height(preloadImage.height).
			css('opacity','0');
			$('.currentImg',parent)
			.animate({width:preloadImage.width,height:preloadImage.height},settings.timeout,
			function(e) 
			{
				$('.currentImg',parent).css('background-image','url(' + images[currentImageNr] + ')');
				$('.nextImg',parent).css('opacity','1');
			});
			$('.nextImg',parent).animate({opacity:'1'},settings.timeout);
		});
		
		function loadImage()
		{	
			preloadImage.src = images[currentImageNr];
			var current = $('ul li',parent).removeClass('selected').eq(currentImageNr).addClass('selected');
			var band = $('.band',parent);
			band.animate({marginLeft:( - current.position().left + band.offset().left) + "px"});
		}
		
		$(".previous", parent).click(function(e)
		{
			currentImageNr <= 0 ? 0 : currentImageNr--;
			loadImage();
		});
		
		$(".next,.nextImg", parent).click(function(e)
		{
			currentImageNr >= images.length - 1 ? images.length - 1 : currentImageNr++;
			loadImage();
		});	
		
		$("div ul li", parent).click(function(e)
		{
			e.preventDefault();
			currentImageNr = $('li',this.parentNode).index(this);
			loadImage();
		});
	});
};

