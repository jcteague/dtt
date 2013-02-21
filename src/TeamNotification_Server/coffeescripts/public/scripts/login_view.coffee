define 'login_view', ['general_view', 'base64',  'form_view','links_view', 'cookie', 'form_template_renderer'], (GeneralView, Base64, FormView, LinksView, Cookie, FormTemplateRenderer) ->

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
                @form_view.render().append_to @$el
                $(@$el.find('input[name=login_password]')[0]).wrap "<span id='password-wrapper' class='inline input-append' style='padding-top:5px'/>"
                $(@$el.find('input[name=login_password]')[0]).attr('style', 'margin-top:5px')
                $($(@$el.find('input[name=login_password]')[0]).parent()).append "<span style='margin-top:5px; margin-right:2px' class='add-on'><a href='##{links[1].href}' title='Forgot password?'>?</a></span>"
            @
            
