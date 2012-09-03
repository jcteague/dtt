define 'login_view', ['general_view', 'base64',  'form_view','links_view'], (GeneralView, Base64, FormView, LinksView) ->

    class LoginView extends GeneralView
    
        id: 'login-container'
        
        initialize :  ->
            @form_view = new FormView(model: @model)
            @links_view = new LinksView(model: @model)
            $.ajaxSetup
                beforeSend: (jqXHR) ->
                    email = $('input[name=username]').val()
                    password = $('input[name=password]').val()
                    jqXHR.setRequestHeader('Authorization', "Basic " + encodeBase64(email + ":" + password))
            @form_view.on 'response:received', @checkLogin
        
        checkLogin: (res) ->
            console.log res
            if res.success is true
                window.location = "client#/user/#{res.user.id}"
        
        render: () ->
            @$el.empty()
            @links_view.render().append_to @$el
            @form_view.render().append_to @$el
            @
            
