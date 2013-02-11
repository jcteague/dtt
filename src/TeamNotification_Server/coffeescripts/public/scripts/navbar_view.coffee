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
                @loginview.on 'messages:display', (messages) =>
                    @trigger 'messages:display', messages
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
                login_form_li = $("<li></li>")
                login_ul = $("""<ul class="nav pull-right"></ul>""")
                login_ul.append login_form_li
                
                @container_nav.append @items
                
                if @loginview?
                    login_form = @loginview.render()
                    
                    login_form.$el.attr('class', 'nav pull-right')
                    login_form.$el.find('form').attr('class','navbar-form')
                    login_form.$el.find('input[type=text]').attr('class','span2')
                    #login_form.$el.find('input[type=text]').attr('style','margin-top:0px; margin-right:3px')
                    login_form.$el.find('input[type=text]').attr('style','margin-right:3px')
                    login_form.$el.find('input[type=password]').attr('class','span2')
                    #login_form.$el.find('input[type=password]').attr('style','margin-top:0px;')
                    #@container_nav.append login_form.$el.find('form')
                    login_form.append_to login_form_li
                    @container_nav.append login_ul
                #@container_nav.append login_ul
                #link.bind 'click', ()->
                 #   $(ddm).toggle()
            @
