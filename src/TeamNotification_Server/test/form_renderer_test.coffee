#mocha = require("mocha")
$ = require("jquery")
should = require("should")
sinon = require("sinon")

FormTemplateRenderer = (collection) ->
    textFieldBuilder = (template) ->
        return [$('<label>', {"for":template.name}), $('<input>',{"type":"text","name":template.name})]
    templateFieldBuilder = (template) ->
        return
    generatorSelector = (fieldType) ->
        return textFieldBuilder if fieldType is 'string'
    form = $('<form>', {action:collection.uri})
    form_templates = collection.template.data
    console.log( form_templates)
    form_templates.forEach (template) ->
        fieldGenerator = generatorSelector(template.type)
        fieldElements = fieldGenerator(template)
        console.log(fieldElements)
        fieldElements.forEach((f) -> form.append(f))
    return form

collection = 
    uri: 'collectionuri'
    data: []
    template:
        data: [{name:'fieldA', type:'string',label:'label'}]

describe "When Rendering the Form frome the collection template", ->
    it "return a form html tag", (done) ->
        form = FormTemplateRenderer(collection)
        form.attr('action').should.equal(collection.uri)
        done()

    it "should render a label with the label field", (done) ->
        form = FormTemplateRenderer(collection)
        childElements = form.children()
        label[0].tagName.should.equal("LABEL")
        done()

