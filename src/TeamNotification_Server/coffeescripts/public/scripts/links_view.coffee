define 'links_view', ['backbone'], (Backbone) ->

    class LinksView extends Backbone.View

        id: 'links'

        initialize: ->
            @model.on 'change:links', @render, @

        render: ->
            @$el.empty()
            if @model.has('links')
                @$el.append('<h1>Links</h1>')
                @append_link link for link in @model.get('links')
            @

        append_to: (parent) ->
            @$el.appendTo parent

        append_link: (link) ->
            @$el.append """
                <p>
                    <a href="##{link.href}">#{link.name}</a>
                </p>
            """
