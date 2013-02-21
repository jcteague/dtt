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
                #console.log links
                
                @$el.append """<div id="form-container" class='inline'><form action="/user/login" class="navbar-form form-inline"><input type="text" name="username" class="span2" placeholder="Email">
                <span class='inline input-append' style='margin-right:3px'><input style='margin-top:5px' id="login_password" type="password" name="login_password" class="span2" placeholder="Password"><span style='margin-top:5px' class='add-on'><a href='##{links[1].href}' title='Forgot password?'>?</a></span></span><input type="submit" class="btn btn-primary"></form></div>"""
                
                #forgot_password_link = @get_link "forgot_password", links #"""<a href='##{links[1].href}' style='display:inline;'> #{links[1].name}?</a>"""
                #console.log forgot_password_link
                #@form_view.render().append_to @$el
                #login_form = @form_view.render() 
                #login_form.$el.attr "class", 'input-append'
                #login_form.append_to @$el
                #$(@$el.find('input[name=login_password]')[0]).wrap("<div id='password-wrapper' class='inline input-append'/>")
                #$('#password-wrapper').append "<span class='add-on'>?</span>"
               # $(@form_view.$el[0]).find('form').append forgot_password_link
            @
            
