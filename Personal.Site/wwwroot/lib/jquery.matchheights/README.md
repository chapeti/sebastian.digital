jquery.equalHeights
===================

jquery.equalHeights is a crazy simple jquery plugin to set a group of elements to the same height.

We end up having to do this on a lot of sites to keep things looking neat no matter what user
generated content gets added to the site, so it made sense to make it a bit more reusable, and we
figured there were other people in the same boat as us, so we made it open source.

Usage
-----

``` javascript
$('.some-css-selector').equalHeights()
```

This will find the tallest of all the items with the provided selector and set all the elements to
the same height as it.

``` javascript
$('.some-wrapper-class').each(function () {
    $(this).find('.some-css-selector').equalHeights()
})
```

If you're DOM structure is such that all your elements that need to be the same height are siblings
you can use the above snippet instead of calling `equalHeights` on every single group of elements on
the page.

Install
-------

### Old School

``` html
<script src="/path/to/jquery.js"></script>
<script src="/path/to/jquery.equalheights.js"></script>
```

### Bower

``` bash
$ bower install jquery-equalheights
```

This will install jquery.equalheights and jquery into your bower directory. Include them into your build process however you see fit.
