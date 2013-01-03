define 'navbar_view', ['general_view','jquery','login_view', 'cookie'], (GeneralView, $, LoginView,cookie) ->
    class NavbarView  extends GeneralView
        id: 'navbar-container'
        items: $('<ul class="nav"></ul>')
        inner_nav: $('<div class="navbar-inner"></div>')
        container_nav: $('<div class="container"></div>')
        
        initialize: ->
            @loginview = null
            if @model.has('login')
                m = @get_model_for('login')
                @loginview = new LoginView(model:m)
        render: ->
            @$el.empty()
            @items.empty()
            @inner_nav.empty()
            @container_nav.empty()
            @$el.attr('class', 'navbar navbar-fixed-top')
            @inner_nav.append @container_nav
            @$el.append @inner_nav
            @items.append '<li><a class="brand" href="/#/">Yackety</a></li>'
            if @model.has('user')
                user = @model.get('user')
                link = $("""<a class="dropdown-toggle" data-toggle="dropdown" href="#"><b>#{@get_field('name',user.data)}</b><b class="caret"></b></a>""")
                dropdownmenu = $("""<ul class="dropdown-menu"><li>#{@get_link('UserEdit', user.links)}</li></ul>""")
                nav = $("""<ul class="nav pull-right"></ul>""")
                lidropdown = $("""<li class="dropdown"></li>""")
                logout_dropdownmenu = $("""<li></li>""")
                dropdownmenu.append logout_dropdownmenu
                nav.append lidropdown
                lidropdown.append link
                lidropdown.append dropdownmenu
                @container_nav.append nav
                
                link.bind 'click', ()->
                    $(dropdownmenu).toggle()
                logout_link = $("""<a href="#/">Logout</a>""")
                logout_link.bind 'click', ()->                    
                    $.cookie("authtoken", "", { expires: -1, path: '/' })
                logout_dropdownmenu.append logout_link
                
                @container_nav.append @items
            else
                login_form = $("<li></li>")
                login_ul = $("""<ul class="nav pull-right"></ul>""")
                dd = $("""<li id="ddToToggle" class="dropdown"></li>""")
                ddm = $("""<ul class="dropdown-menu"></ul>""")
                link = $("""<a class="dropdown-toggle" data-toggle="dropdown" href="#">Sign in <b class="caret"></b></a>""")
                @items.append '<li><a href="#">Features</a></li>'
                @items.append '<li><a href="#">Contact us</a></li>'
                @container_nav.append @items
                
                ddm.append login_form
                dd.append link
                dd.append ddm
                login_ul.append dd
                if @loginview?
                    @loginview.render().append_to login_form
                @container_nav.append login_ul
                link.bind 'click', ()->
                    $(ddm).toggle()
            @
