module ("PageComponent", package.seeall)

function New(onPageChanged)
    local t = {cur = 1, imax = 1};

    function t:SetMax(n)
        self.imax = n;
    end;
    
    function t:PrevPage()
        if (self.cur > 0) then
            self.cur = self.cur - 1;
            
            onPageChanged(self.cur);
        end
    end;

    function t:NextPage()
        if (self.cur < self.imax) then
            self.cur = self.cur + 1;
            
            onPageChanged(self.cur);
        end
    end;
    
    function t:FirstPage()
        return self.cur == 1;
    end
    
    function t:LastPage()
        return self.cur == self.imax;
    end
    
    t.__index = t;
    
    return t;
end