using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Media;

namespace Quizi
{
    class playsounds
    {
        
        public void playlock()
        {
            
            try
            {
                SoundPlayer player = new SoundPlayer(playsound.LockTone);
                player.Play();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public void playTheme()
        {

        }
        
    }
}
