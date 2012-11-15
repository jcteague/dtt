define 'form_view', ['general_view', 'form_template_renderer','base64', 'config'], (GeneralView, FormTemplateRenderer, Base64, config) ->

    class FormView extends GeneralView

        id: 'form-container'

        events:
            'submit': 'submit_form'

        initialize: ->
            @model.on 'change:template', @render, @
            @form_template_renderer = new FormTemplateRenderer()

        render: ->
            @$el.empty()
            if @model.has('template')
                @$el.append(@form_template_renderer.render(@model.attributes))
            @delegateEvents(@events)
            @

        append_to: (parent) ->
            @$el.appendTo parent

        submit_form: (event) ->
            event.preventDefault()
            data = {}
            $('input').not(':submit').each () ->
                $current = $(this)
                data[$current.attr('name')] = $current.val()
            $('textarea').each () ->
                $current = $(this)
                data[$current.attr('name')] = $current.val()
            $('select').each () ->
                $current = $(this)
                data[$current.attr('name')] = $current.val()
            callback = (res) => 
                @trigger 'response:received', res
                if res.server_messages?
                    if res.redirect? && res.redirect
                        window.location = "##{res.link}"
                    if res.link?
                        res.server_messages.push "You can view the new resource <a href='##{res.link}'>here</a>"
                    @trigger 'messages:display', res.server_messages 

            url = "#{config.api.url}#{@$('form').attr('action')}"
            parameters = {
                type: 'POST'
                data: data
                url: url
                success: callback
                error: (d) -> return
            }

            if $.cookie('authtoken')?
                parameters.beforeSend = (jqXHR) ->
                    authToken = $.cookie 'authtoken'
                    jqXHR.setRequestHeader('Authorization', authToken )
                    jqXHR.withCredentials = true



            $.ajax parameters

            $('form').get(0).reset()
