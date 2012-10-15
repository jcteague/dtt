define 'login_view', ['general_view', 'base64',  'form_view','links_view', 'cookie'], (GeneralView, Base64, FormView, LinksView, Cookie) ->

    class LoginView extends GeneralView
    
        id: 'login-container'
        
        initialize : ->
            @form_view = new FormView(model: @model)
            @links_view = new LinksView(model: @model)
            $.ajaxSetup
                beforeSend: (jqXHR) ->
                    email = $('input[name=username]').val()
                    password = $('input[name=password]').val()
                    authToken = "Basic " + encodeBase64(email + ":" + password)
                    console.log 'this is the setup', authToken
                    jqXHR.setRequestHeader('Authorization', authToken )
                    jqXHR.withCredentials = true
            @form_view.on 'response:received', @check_login
        
        check_login: (res) ->
            if res.success is true
                console.log res.user.id, res.user.email, res.user.authtoken
                getIn = () ->
                    $.cookie("authtoken", res.user.authtoken, { expires: 1, path: '/' })
                    #redirect = "client#/user/#{res.user.id}"
                    redirect = "#/user/#{res.user.id}"
                    if(window.location.href.lastIndexOf('/user/login') > -1)
                        window.location.href = redirect
                    window.location.reload(true)
                setTimeout getIn, 500

        render: ->
            @$el.empty()
            @links_view.render().append_to @$el
            @form_view.render().append_to @$el
            @
            
