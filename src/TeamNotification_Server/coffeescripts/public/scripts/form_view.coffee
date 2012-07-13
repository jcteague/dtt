define 'form_view', ['backbone', 'form_template_renderer'], (Backbone, FormTemplateRenderer) ->

    class FormView extends Backbone.View

        id: 'form-container'

        events:
            'submit': 'submit_form'

        initialize: ->
            @form_template_renderer = new FormTemplateRenderer()

        render: ->
            @$el.empty()
            if @model? and @model.template
                @$el.append('<h1>Form</h1>')
                @$el.append(@form_template_renderer.render(@model))
            @delegateEvents(@events)
            @

        update: (template) ->
            @model = template

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
            $.post @$('form').attr('action'), data, (res) => 
                @trigger 'messages:display', res.messages if res.messages?
                @trigger 'response:received', res
            $('form').get(0).reset()
