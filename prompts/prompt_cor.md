I don't like how each manager does the validation check. Take #file:DietManager.cs for example, here you can see there are many if, but sometimes these checks are the same for #file:PlanManager.cs and could be the same for other managers.
I also don't like #file:BaseManager.cs, I think there should be a design pattern that i can use to run common validation logic on different managers.

Find what are the type of validation I make, after that you should check for the best design patterns among these sources #file:prompt_design_patterns_instructions.md that fit the ddd architecture.

This is very important:

1. Sometimes the validation are the same for different managers, so couple the context based on the validation, not on the manager.
2. The validation should return a BusinessResponse
3. do not create any helper static method like mappers
4. the managers will inject validation sets (for each CRUD operation), which is a list of validation that managers has to do in each method. The manager will iterate the list and break if some in the pipeline returns a business response with Data set to null.
5. do not use reflection or expressions, make the method abstract if needed or use generics.

You should return:

- a mermaid with your proposal
- a sample of the implementation for one of the manager (ie. #file:DietManager.cs)
