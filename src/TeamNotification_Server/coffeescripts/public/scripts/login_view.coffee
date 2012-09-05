define 'login_view', ['general_view', 'base64',  'form_view','links_view', 'cookie'], (GeneralView, Base64, FormView, LinksView, Cookie) ->

    class LoginView extends GeneralView
    
        id: 'login-container'
        
        initialize :  ->
            @form_view = new FormView(model: @model)
            @links_view = new LinksView(model: @model)
            $.ajaxSetup
                beforeSend: (jqXHR) ->
                    email = $('input[name=username]').val()
                    password = $('input[name=password]').val()
                    authToken = "Basic " + encodeBase64(email + ":" + password)
                    jqXHR.setRequestHeader('Authorization', authToken )
            @form_view.on 'response:received', @checkLogin
        
        checkLogin: (res) ->
            if res.success is true
                $.cookie("authtoken", res.user.authtoken, { expires: 1, path: '/' })
                redirect = "client#/user/#{res.user.id}"
                if(window.location.href.contains('user/login')
                    window.location.href = redirect
                window.location.reload(true)
        
        render: () ->
            @$el.empty()
            @links_view.render().append_to @$el
            @form_view.render().append_to @$el
            @
            
