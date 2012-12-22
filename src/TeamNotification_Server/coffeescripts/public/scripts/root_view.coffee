define 'root_view', ['general_view','config','navbar_view','breadcrumb_view','form_view','login_view'], (GeneralView, config, NavbarView, BreadcrumbView, FormView, LoginView) ->

    class RootView extends GeneralView

        id: 'root-container'
        registerview: $("""<div class='span2'></div>""")
        left_form: $("""<div class='span6'></div>""")
        row: $("""<div class='row-fluid well container'></div>""")
        initialize: ->
            @navbar = new NavbarView(model:@model)
            @breadcrumb = new BreadcrumbView(model:@model)
        render: ->
            @$el.empty()
            @registerview.empty()
            @left_form.empty()
            @row.empty()
            if @model.has('root') 
                @navbar.render().append_to @$el
                links = @model.get('root').links
                reg = @model.get('registration')
                login = @model.get('login')
                
                @model.attributes = reg
                @formview = new FormView(model:@model)
                @formview.on 'messages:display', (messages) =>
                    $('#server-response-container').html("""<div class="alert alert-info" id='#notification'><button type="button" class="close" data-dismiss="alert">x</button>"""+messages[0]+"""</div>""" )
                @$el.append @row
                @row.append @left_form
                @registerview.append "<h3>Sing up</h1>"
                @formview.render().append_to @registerview
                @row.append @registerview
                @left_form.append """<h1>Welcome to Yackety</h1> Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. Fusce nec tellus sed augue semper porta. Mauris massa. Vestibulum lacinia arcu eget nulla. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Curabitur sodales ligula in libero. 

Sed dignissim lacinia nunc. Curabitur tortor. Pellentesque nibh. Aenean quam. In scelerisque sem at dolor. Maecenas mattis. Sed convallis tristique sem. Proin ut ligula vel nunc egestas porttitor. Morbi lectus risus, iaculis vel, suscipit quis, luctus non, massa. Fusce ac turpis quis ligula lacinia aliquet. Mauris ipsum. Nulla metus metus, ullamcorper vel, tincidunt sed, euismod in, nibh. Quisque volutpat condimentum velit."""
            @
        anchor_click_handler:(obj)->
            $('.active').attr('class','');
            
            
        append_to: (parent) ->
            @$el.appendTo parent
