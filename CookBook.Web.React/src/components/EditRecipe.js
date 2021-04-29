import React, { Component } from 'react';
import { Button, Container, Heading, Form } from 'react-bulma-components';
import { updateRecipe, uploadFiles, recipeStore } from '../models/recipes';
import { InstructionsInput } from './RecipeForm/InstructionsInput';
import { IngredientsInput } from './RecipeForm/IngredientsInput';
import { TitleInput } from './RecipeForm/TitleInput';
import { DescriptionInput } from './RecipeForm/DescriptionInput';
import { NumberOfServingsInput } from './RecipeForm/NumberOfServingsInput';

import { Form as FinalForm } from 'react-final-form';
import { mapErrorsToObject } from 'services/businessError';

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

    async handleSubmit() {
        try {
            await updateRecipe(this.state);
            this.props.history.push("/");
        }
        catch (e) {
            if (e.response.status === 400)
                return mapErrorsToObject(e.response.data.businessErrors);

            throw e;
        }
    }

    render() {
        return (
            <Container>
                <Heading size={3}>Edit Recipe '{this.state.title}'</Heading>
                <FinalForm
                    onSubmit={this.handleSubmit}
                    render={({ handleSubmit }) => (
                        <form onSubmit={handleSubmit}>
                            <TitleInput title={this.state.title} handleChange={this.handleChange} />
                            <NumberOfServingsInput servings={this.state.servings} handleChange={this.handleChange} />
                            <DescriptionInput description={this.state.description} handleChange={this.handleChange} />
                            <InstructionsInput instructions={this.state.instructions} handleChange={this.handleChange} />
                            <IngredientsInput ingredients={this.state.ingredients} handleChange={this.handleChange} />
                            <Button className="is-success" type="submit">Update</Button>
                        </form>
                    )} />
            </Container>
        );
    }
}