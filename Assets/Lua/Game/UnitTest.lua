module("UnitTest", package.seeall)



function OnInit()

end

function Test()
	GameEmail.RequestGetEmails();
	GameEmail.RequestEmailDetail(1);
	GameEmail.RequestDeleteEmail(1);

	Hero.ReqHeroTupo(1);
	Hero.ReqHeroAdv(1);
end



