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
                #@$el.append('<h2>Form</h2>')
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
                if res.messages?
                    if res.redirect? && res.redirect
                        window.location = "##{res.link}"
                    if res.link?
                        res.messages.push "You can view the new resource <a href='##{res.link}'>here</a>"
                    @trigger 'messages:display', res.messages 

            url = "#{config.api.url}#{@$('form').attr('action')}"
            console.log 'Posting to', url
            parameters = {
                type: 'POST'
                data: data
                url: url
                success: callback
                error: (d) -> console.log('Error')
            }

            if $.cookie('authtoken')?
                parameters.beforeSend = (jqXHR) ->
                    authToken = $.cookie 'authtoken'
                    jqXHR.setRequestHeader('Authorization', authToken )
                    jqXHR.withCredentials = true

            console.log $.cookie('authtoken')

            console.log 'parameters is', parameters

            $.ajax parameters

            $('form').get(0).reset()
