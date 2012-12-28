define 'login_view', ['general_view', 'base64',  'form_view','links_view', 'cookie'], (GeneralView, Base64, FormView, LinksView, Cookie) ->

    class LoginView extends GeneralView
    
        id: 'login-container'
        
        initialize : ->
            @form_view = new FormView(model: @model)
            $.ajaxSetup
                beforeSend: (jqXHR) ->
                    email = $('input[name=username]').val()
                    password = $('input[name=login_password]').val()
                    authToken = "Basic " + encodeBase64(email + ":" + password)
                    jqXHR.setRequestHeader('Authorization', authToken )
                    jqXHR.withCredentials = true
            @form_view.on 'response:received', @check_login
            @form_view.on 'messages:display', (messages) =>
                @trigger 'messages:display', messages
        
        check_login: (response) ->
            res = $.parseJSON response
            res = response unless res?
            if typeof(res.success)!='undefined' && res.success is true
                getIn = () ->
                    $.cookie("authtoken", res.user.authtoken, { expires: 1, path: '/' })
                    redirect = "#/user"
                    if(window.location.href.lastIndexOf('/user/login') > -1)
                        window.location.href = redirect
                    window.location.reload(true)
                setTimeout getIn, 500
            else
                @trigger 'messages:display', res.server_messages

        render: ->
            @$el.empty()
            if @model.has('links')
                links = @model.get('links')
                forgot_password_link = @get_link "forgotPassword", links #"""<a href='##{links[1].href}' style='display:inline;'> #{links[1].name}?</a>"""
                @form_view.render().append_to @$el
                $(@form_view.$el[0]).find('form').append forgot_password_link
            @
            
