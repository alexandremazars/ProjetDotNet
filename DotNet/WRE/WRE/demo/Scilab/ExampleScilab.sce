
// Please replace the path below by the path of the directory where lies the file wre.sci.
cd "C:\sophie\RaisePartner\Produits\ScilabAPI"

A=eye(5,5);
p=5;
exec("wre.sci");
Cineq=zeros(1,1);
bineql=zeros(Cineq);
binequ=zeros(Cineq);
Ceq=zeros(1,1);
beq=zeros(Cineq);

[Xsdls, info]=WREmodelingSDLS(p,A, 0, Ceq,beq, 0, Cineq,bineql,binequ, 1e-7,1e-6);

