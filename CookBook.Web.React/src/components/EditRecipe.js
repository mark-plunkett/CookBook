import React, { Component } from 'react';
import { Button, Form } from 'react-bulma-components';
import { updateRecipe, uploadFiles, recipeStore } from '../models/recipes';

const { Input, Field, Control, Label, Textarea, InputFile } = Form;

export class EditRecipe extends Component {

    constructor(props) {
        super(props);
        this.id = props.match.params.id;
        this.state = {};
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    async componentDidMount() {
        this.setState(await recipeStore.get(this.id));
    }

    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
        this.setState({
            [name]: value
        });
    }

    onFileChange = async event => {
        const recipeAlbumDocumentID = await uploadFiles(event.target.files);
        this.setState({
            ...this.state,
            recipeAlbumDocumentID: recipeAlbumDocumentID
        });
    }

    async handleSubmit(event) {
        event.preventDefault();
        await updateRecipe(this.state);
        this.props.history.push("/recipes/view/" + this.state.id);
    }

    render() {
        return (
            <div>
                <h1>Edit Recipe '{this.state.title}'</h1>
                <form onSubmit={this.handleSubmit}>
                    <Field>
                        <Control>
                            <Label>Title</Label>
                            <Input
                                type="text"
                                name="title"
                                value={this.state.title}
                                onChange={this.handleChange} />
                        </Control>
                    </Field>
                    <Field>
                        <Control>
                            <Label>Number of servings</Label>
                            <Input
                                type="number"
                                min="1"
                                name="servings"
                                value={this.state.servings}
                                onChange={this.handleChange} />
                        </Control>
                    </Field>
                    <Field>
                        <Label>Pictures</Label>
                        <InputFile name="pictures" boxed inputProps={{ multiple: true }} onChange={this.onFileChange}>
                        </InputFile>
                    </Field>
                    <Field>
                        <Label>Description</Label>
                        <Control>
                            <Textarea
                                name="description"
                                value={this.state.description}
                                onChange={this.handleChange} />
                        </Control>
                    </Field>
                    <Field>
                        <Label>Instructions</Label>
                        <Control>
                            <Textarea
                                name="instructions"
                                value={this.state.instructions}
                                onChange={this.handleChange} />
                        </Control>
                    </Field>
                    <Field>
                        <Label>Ingredients</Label>
                        <Control>
                            <Textarea
                                name="ingredients"
                                value={this.state.ingredients}
                                onChange={this.handleChange} />
                        </Control>
                    </Field>
                    <Button className="is-primary" onClick={this.handleSubmit}>Update</Button>
                </form>
            </div>
        );
    }
}