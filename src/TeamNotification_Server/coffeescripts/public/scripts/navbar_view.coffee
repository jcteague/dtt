define 'navbar_view', ['general_view','jquery','login_view'], (GeneralView, $, LoginView) ->
    class NavbarView  extends GeneralView
        id: 'navbar-container'
        items: $('<ul class="nav"></ul>')
        inner_nav: $('<div class="navbar-inner"></div>')
        container_nav: $('<div class="container"></div>')
        
        login_form: $("<li id='login-form'></li>")
        login_ul: $("""<ul class="nav pull-right"></ul>""")
        dd: $("""<li id="ddToToggle" class="dropdown"></li>""")
        ddm: $("""<ul class="dropdown-menu"></ul>""")
        initialize: ->
            m = 
                get: @model.get
                has: @model.has
                on: @model.on
                attributes: @model.get('login')
                
            @loginview = new LoginView(model:m)
        render: ->
            @$el.empty()
            @items.empty()
            @inner_nav.empty()
            @container_nav.empty()
            @$el.attr('class', 'navbar navbar-fixed-top')
            @inner_nav.append @container_nav
            @$el.append @inner_nav
            @items.append '<li><a class="brand" href="#">Yackety</a></li>'
            if @model.has('user')
                @container_nav.append '<ul class="nav pull-right"><li><a href="#/user"><b>Username here</b></a></li></ul>'
                @container_nav.append @items
            else
                @items.append '<li><a href="#">Features</a></li>'
                @items.append '<li><a href="#">Contact us</a></li>'
                @container_nav.append @items
                
                @ddm.append @login_form
                @dd.append """<a class="dropdown-toggle" data-toggle="dropdown" href="#">Sign in <b class="caret"></b></a>"""
                @dd.append @ddm
                @login_ul.append @dd
                @loginview.render().append_to @login_form
                @container_nav.append @login_ul
                $('.dropdown-toggle').dropdown()
            @
