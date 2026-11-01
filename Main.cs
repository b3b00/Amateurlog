using System.Collections.Immutable;
using Amateurlog;
using Amateurlog.Machine;

var list = @"
    type list(X) :-
        cons(X, list(X)),
        nil.
    
    type data :-
        foo,
        bar.

    set(X, X).

    first(cons(X, Y), X).

    last(cons(X, nil), X).
    last(cons(X, Y), Z) :- last(Y, Z).

    main() :-
        set(cons(foo, cons(bar, nil)), List),
        first(List, X),
        last(List, Y),
        dump(X),
        dump(Y),
        exit().
";

	var dinner = @"

type people :-
        bonnie,
        clyde.

type meal :-
        steak,
        vegetables.

likes(Person, Food) :- loves(Person, Food).

hugry(clyde).
hungry(bonnie).

loves(clyde,steak).
loves(bonnie,steak).


wants(Person, Food) :- hungry(Person), likes(Person, Food).
wants(Person, Food) :- loves(Person, Food).

dinner(Person1, Person2, Food) :- wants(Person1, Food), wants(Person2, Food).

main() :- 
    dinner(X,Y,steak),
    dump(X),
    dump(Y),
    exit().
";
var ast = PrologParser.ParseProgram(list);
var result = new TypeChecker().Infer(ast);
var program = Compiler.Compile(ast.Decls.OfType<Rule>().ToImmutableArray());
var machine = new Machine(program);
machine.Run();

ast = PrologParser.ParseProgram(dinner);
result = new TypeChecker().Infer(ast);
program = Compiler.Compile(ast.Decls.OfType<Rule>().ToImmutableArray());
machine = new Machine(program);
machine.Run();
