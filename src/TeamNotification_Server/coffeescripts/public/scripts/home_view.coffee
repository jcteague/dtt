define 'home_view', ['general_view', 'base64',  'form_view','links_view', 'cookie', 'navbar_view'], (GeneralView, Base64, FormView, LinksView, Cookie, NavbarView) ->

    class HomeView extends GeneralView
    
        id: 'home-container'
        
        initialize : ->
            
        render: ->
            @$el.empty()
            #@navbar.render().append_to @$el
            $("#Signin").attr('class','pull-right') 
            #@links_view.render().append_to @$el
            #@form_view.render().append_to @$el
            @
            
